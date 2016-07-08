using Akka.Persistence;
using AkkaPlayground.Core.Entities;
using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Events;
using System;
using Akka;
using Akka.Actor;
using System.Collections.Generic;
using AkkaPlayground.Messages.Messages;
using System.Linq;
using Akka.Cluster.Sharding;
using AkkaPlayground.Messages;
using AkkaPlayground.Core.Actors.Views;
using AkkaPlayground.Projections;
using AkkaPlayground.Messages.Entities;

namespace AkkaPlayground.Core.Actors
{
    public class Chat : ReceivePersistentActor
    {
        private ChatEntity State;
        private IActorRef _userRegion;

        private IActorRef _chatView;
        

        public Chat()
        {
            Context.Become(UnInitialized);
            RecoverAny(UpdateState);

            ClusterSharding clusterSharding = ClusterSharding.Get(Context.System);
            _userRegion = clusterSharding.ShardRegion(typeof(User).Name);

            _chatView = Context.ActorOf(Props.Create(() => new ChatView()));
            

            if (_userRegion == null)
            {
                _userRegion = clusterSharding.Start(
                                typeName: typeof(User).Name,
                                entityProps: Props.Create<User>(),
                                settings: ClusterShardingSettings.Create(Context.System),
                                messageExtractor: new MessageExtractor(10));
            }
        }

        public override string PersistenceId { get { return Self.Path.Name; } }

        private void UnInitialized(object message)
        {
            message.Match()
                .With<CreateChatCommand>(cmd =>
                {
                    List<GetUserByIdResult> users = new List<GetUserByIdResult>();
                    foreach(Guid cmdParticipantItem in cmd.Participants)
                    {
                        var envelop = new ShardEnvelope(cmdParticipantItem.ToString(), new GetUserById(cmdParticipantItem));
                        GetUserByIdResult contactUser = _userRegion.Ask<GetUserByIdResult>(envelop).Result;
                        users.Add(contactUser);
                    }

                    List<ChatParticipant> chatParticipants =
                        users.Select(x => new ChatParticipant(x.Id,  x.Login, x.UserName)).ToList();

                    ChatParticipant creator = users.Where(x => x.Id == cmd.Creator)
                        .Select(x => new ChatParticipant(x.Id, x.Login, x.UserName)).FirstOrDefault();

                    var @event = new ChatCreatedEvent(cmd.Id, creator, chatParticipants);
                    Persist<ChatCreatedEvent>(@event, UpdateState);
                });
        }

        private void Initialized(object message)
        {
            message.Match()
                .With<AddMessageToChatCommand>(mes =>
                {
                    var envelop = new ShardEnvelope(mes.Author.ToString(), new GetUserById(mes.Author));
                    GetUserByIdResult contactUser = _userRegion.Ask<GetUserByIdResult>(envelop).Result;

                    Guid messageId = Guid.NewGuid();
                    var messaegeAdded = new ChatMessageAddedEvent(messageId, mes.ChatId, 
                          DateTime.UtcNow, mes.Message, new ChatParticipant(contactUser.Id, contactUser.Login, contactUser.UserName));
                    Persist(messaegeAdded, UpdateState);
                });

        }

        private void UpdateState(object e)
        {
            e.Match().With<ChatCreatedEvent>(x =>
            {
                State = new ChatEntity(x.Id);
                State.Participants = x.Participants;
                Context.Become(Initialized);
                if (!IsRecovering)
                {
                    ResendToView(x);

                    foreach(var user in x.Participants)
                    {
                        var envelop = new ShardEnvelope(user.Id.ToString(), x);
                        _userRegion.Tell(envelop);
                    }
                }
            });

            e.Match().With<ChatMessageAddedEvent>(x =>
            {
                var chatLogEntity = new ChatLogEntity(x.MessageId, x.Author, x.Message, DateTime.UtcNow);
                State.Log.Add(chatLogEntity);
                if (!IsRecovering)
                {
                    ResendToView(x);
                    foreach (var user in State.Participants)
                    {
                        var envelop = new ShardEnvelope(user.Id.ToString(), x);
                        _userRegion.Tell(envelop);
                    }
                }
            });
        }

        private void ResendToView(object x)
        {
            _chatView.Tell(x);
        }
    }
}
