using Akka.Persistence;
using AkkaPlayground.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Events;
using Akka.Actor;
using AkkaPlayground.Messages.Messages;
using AkkaPlayground.Core.Actors.Views;
using Akka.Cluster.Sharding;
using AkkaPlayground.Messages;

namespace AkkaPlayground.Core.Actors
{
    public class User : ReceivePersistentActor
    {
        private UserEntity State = null;
        private IActorRef _userView;
        private IActorRef _userContactsView;
        private IActorRef _region;

        public override string PersistenceId { get { return Self.Path.Name; } }

        public User()
        {
            Context.Become(Unregistered);
            RecoverAny(UpdateState);

            _userView = Context.ActorOf(Props.Create(() => new UserView(PersistenceId)), "users-projection-" + PersistenceId);
            _userContactsView = Context.ActorOf(Props.Create(() => new UserContactsView(PersistenceId)),
                "user-contact-list-projection-" + PersistenceId);

            ClusterSharding clusterSharding = ClusterSharding.Get(Context.System);
            _region = clusterSharding.ShardRegion(typeof(User).Name);
            if (_region == null)
            {
                _region = clusterSharding.Start(
                                typeName: typeof(User).Name,
                                entityProps: Props.Create<User>(),
                                settings: ClusterShardingSettings.Create(Context.System),
                                messageExtractor: new MessageExtractor(10));
            }
        }


        protected void Unregistered(object message)
        {
            message.Match()
                .With<RegisterUserCommand>(register =>
                {
                    var userRegistered = new UserRegisteredEvent(register.Id, register.Login, register.Email);
                    Persist(userRegistered, UpdateState);
                });
        }


        protected void Initialized(object message)
        {
            message.Match()
                .With<ChangeUserNameEmailCommand>(cmd =>
                {
                    var @event = new UserNameEmailChangedEvent(cmd.Id, cmd.Name, cmd.Email);
                    Persist(@event, UpdateState);
                })
                .With<SubscribeToUserCommand>(cmd =>
                {
                    if (State.Id != cmd.ContactUserId && !State.SubscribedToList.Any(x => x == cmd.ContactUserId))
                    {
                        var envelop = new ShardEnvelope(cmd.ContactUserId.ToString(),
                            new GetUserById(cmd.ContactUserId));
                        GetUserByIdResult contactUser = _region.Ask<GetUserByIdResult>(envelop).Result;

                        var @event = new SubscribedToUserEvent(cmd.UserId, cmd.ContactUserId, contactUser.Login, contactUser.Name);
                        Persist(@event, UpdateState);
                    }
                })
                .With<GetUserById>(x =>
                {
                    Sender.Tell(new GetUserByIdResult(State.Login, State.Name));
                });
                //.With<SubscribedToUserEvent>(cmd =>
                //{
                //    if (!State.FollowersList.Any(x => x == cmd.ContactUserId))
                //    {
                //        var @event = new UserAddedToFollowersEvent(cmd.ContactUserId, cmd.UserId);
                //        Persist<UserAddedToFollowersEvent>(@event, UpdateState);
                //    }
                //});
                //.With<GetUserSubscribedToList>(mes =>
                //{
                //    Sender.Tell(null);
                //    //userView.Ask<SubscribedToListResult>(mes).PipeTo(Sender);
                //})
                //.With<ChatMessageAddedEvent>(mes =>
                //{
                //    if (mes.Author != State.Id)
                //    {
                //    }
                //    Persist<UserChatMessageAddedEvent>(new UserChatMessageAddedEvent(mes.ChatId, mes.Author, mes.Message, mes.Date), UpdateState);
                //    //userView.Tell(new Update());
                //})
                //.With<ChatCreatedEvent>(mes =>
                //{
                //    if (mes.Participants.Contains(State.Id))
                //    {
                //        var @event = new UserAddedToChatEvent(mes.Id, mes.Participants);
                //        Persist<UserAddedToChatEvent>(@event, UpdateState);
                //    }
                //})
                //.With<GetPrivateChatWithUser>(mes =>
                //{
                //    UserChatEntity userPrivateChat = State.Chats.FirstOrDefault(x => x.Participants.Count == 2 && x.Participants.Any(y => y == mes.TargetUserId));
                //    if (userPrivateChat != null)
                //    {
                //        Sender.Tell(new UserPrivateChatReult(userPrivateChat.ChatId));
                //    }
                //    else
                //    {
                //        Sender.Tell(new UserPrivateChatReult(null));
                //    }
                //})
                //.With<GetUserChats>(mes =>
                //{
                //    //userView.Ask<UserChatsResult>(mes).PipeTo(Sender);
                //});
        }

        protected void UpdateState(object e)
        {
            e.Match()
            .With<UserRegisteredEvent>(x =>
            {
                State = new UserEntity(x.Id, x.Login, x.Login, x.Email);
                if (!IsRecovering)
                {
                    Context.System.EventStream.Publish(x);
                }
                Context.Become(Initialized);
            })
            .With<UserNameEmailChangedEvent>(x =>
            {
                State.Name = x.Name;
                State.Email = x.Email;
                if (!IsRecovering)
                {
                    Context.System.EventStream.Publish(x);
                }
            })
            .With<SubscribedToUserEvent>(x =>
            {
                State.SubscribedToList.Add(x.ContactUserId);
                _userView.Tell(new Update(isAwait: true));

                if (!IsRecovering)
                {
                    Context.System.EventStream.Publish(x);
                }
            });
            //.With<UserAddedToFollowersEvent>(x =>
            //{
            //    State.FollowersList.Add(x.TargetUserId);
            //    //userView.Tell(new Update(isAwait: true));
            //})
            //.With<UserAddedToChatEvent>(x =>
            //{
            //    State.Chats.Add(new UserChatEntity() { ChatId = x.ChatId, Participants = x.Participants });
            //});
        }
        
    }
}


