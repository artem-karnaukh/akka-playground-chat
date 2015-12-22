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

namespace AkkaPlayground.Core.Actors
{
    public class User : ReceivePersistentActor
    {
        private readonly Guid _id;
        private UserEntity State = null;
        private IActorRef notificationPusher;
        //private IActorRef userView;

        public User(Guid id)
        {
            _id = id;
            Context.Become(Unregistered);
            RecoverAny(UpdateState);

            notificationPusher = Context.ActorOf(Props.Create(() => new UserMobileNotificationPusher()), "mobile-pusher-"+_id.ToString());
            //userView = Context.ActorOf(Props.Create(() => new UserView(_id)), "user-view-" + _id.ToString());
        }

        public override string PersistenceId
        {
            get { return _id.ToString(); }
        }

        protected void Unregistered(object message)
        {
            message.Match()
                .With<RegisterUserCommand>(register =>
                {
                    var userRegistered = new UserRegisteredEvent(_id, register.Name, register.Email);
                    Persist<UserRegisteredEvent>(userRegistered, UpdateState);
                    
                });
        }

        protected override bool AroundReceive(Receive receive, object message)
        {
            return base.AroundReceive(receive, message);
        }

        protected void Initialized(object message)
        {
            message.Match()
                .With<ChangeUserNameEmailCommand>(change =>
                {
                    var userNameChanged = new UserNameEmailChangedEvent(change.Id, change.Name, change.Email);
                    Persist<UserNameEmailChangedEvent>(userNameChanged, UpdateState);
                })
                .With<SubscribeToUserCommand>(add =>
                {
                    if (!State.SubscribedToList.Any(x => x == add.TargetUserId))
                    {
                        var @event = new SubscribedToUserEvent(add.UserId, add.TargetUserId);
                        Persist<SubscribedToUserEvent>(@event, UpdateState);
                    }
                })
                .With<SubscribedToUserEvent>(cmd =>
                {
                    if (!State.FollowersList.Any(x => x == cmd.TargetUserId))
                    {
                        var @event = new UserAddedToFollowersEvent(cmd.TargetUserId, cmd.UserId);
                        Persist<UserAddedToFollowersEvent>(@event, UpdateState);
                    }
                })
                .With<GetUserSubscribedToList>(mes =>
                {
                    Sender.Tell(null);
                    //userView.Ask<SubscribedToListResult>(mes).PipeTo(Sender);
                })
                .With<ChatMessageAddedEvent>(mes =>
                {
                    if (mes.Author != State.Id)
                    {
                        notificationPusher.Forward(mes);
                    }
                    Persist<UserChatMessageAddedEvent>(new UserChatMessageAddedEvent(mes.ChatId, mes.Author, mes.Message, mes.Date), UpdateState);
                    //userView.Tell(new Update());
                })
                .With<ChatCreatedEvent>(mes =>
                {
                    if (mes.Participants.Contains(State.Id))
                    {
                        var @event = new UserAddedToChatEvent(mes.Id, mes.Participants);
                        Persist<UserAddedToChatEvent>(@event, UpdateState);
                    }
                })
                .With<GetPrivateChatWithUser>(mes =>
                {
                    UserChatEntity userPrivateChat = State.Chats.FirstOrDefault(x => x.Participants.Count == 2 && x.Participants.Any(y => y == mes.TargetUserId));
                    if (userPrivateChat != null)
                    {
                        Sender.Tell(new UserPrivateChatReult(userPrivateChat.ChatId));
                    }
                    else
                    {
                        Sender.Tell(new UserPrivateChatReult(null));
                    }
                })
                .With<GetUserChats>(mes =>
                {
                    //userView.Ask<UserChatsResult>(mes).PipeTo(Sender);
                });
        }

        protected void UpdateState(object e)
        {
            e.Match()
            .With<UserRegisteredEvent>(x =>
            {
                State = new UserEntity(x.Id, x.Name, x.Email);
                if (!IsRecovering)
                {
                    Context.System.EventStream.Publish(x);
                }
                Context.Become(Initialized);

                Sender.Tell(x);
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
                State.SubscribedToList.Add(x.TargetUserId);
                Sender.Tell(x);

                if (!IsRecovering)
                {
                    string path = "user/user-buckets/*/" + x.TargetUserId.ToString();
                    Context.System.ActorSelection(path).Tell(x);
                }
                //userView.Tell(new Update(isAwait: true));

            })
            .With<UserAddedToFollowersEvent>(x =>
            {
                State.FollowersList.Add(x.TargetUserId);
                //userView.Tell(new Update(isAwait: true));
            })
            .With<UserAddedToChatEvent>(x =>
            {
                State.Chats.Add(new UserChatEntity() { ChatId = x.ChatId, Participants = x.Participants });
            });
        }
        
    }
}


