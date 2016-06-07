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
                .With<ChangeUserNameEmailCommand>(change =>
                {
                    var @event = new UserNameEmailChangedEvent(change.Id, change.Name, change.Email);
                    Persist(@event, UpdateState);
                })
                .With<SubscribeToUserCommand>(add =>
                {
                    if (!State.SubscribedToList.Any(x => x == add.ContactUserId))
                    {
                        //var s = ClusterSharding.Get(Context.System);
                        //var set = ClusterShardingSettings.Create(Context.System);
                        //var region = s.Start(
                        //                typeName: typeof(User).Name,
                        //                entityProps: Props.Create<User>(),
                        //                settings: set,
                        //                messageExtractor: new MessageExtractor(10));

                        //var envelop = new ShardEnvelope(add.ContactUserId.ToString(), 
                        //    new GetUserById(add.ContactUserId));
                        //GetUserByIdResult contactUser = region.Ask<GetUserByIdResult>(envelop).Result;

                        //var @event = new SubscribedToUserEvent(add.UserId, add.ContactUserId, contactUser.Login, contactUser.Name);
                        //Persist(@event, UpdateState);
                    }
                })
                .With<GetUserById>(x =>
                {
                    Sender.Tell(new GetUserByIdResult(State.Login, State.Name));
                });
                

                //.With<SubscribedToUserEvent>(cmd =>
                //{
                //    if (!State.FollowersList.Any(x => x == cmd.TargetUserId))
                //    {
                //        var @event = new UserAddedToFollowersEvent(cmd.TargetUserId, cmd.UserId);
                //        Persist<UserAddedToFollowersEvent>(@event, UpdateState);
                //    }
                //})
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
                //userView.Tell(new Update(isAwait: true));
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


