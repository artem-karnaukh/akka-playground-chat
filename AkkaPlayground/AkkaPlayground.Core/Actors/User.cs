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
        UserEntity State = null;
        IActorRef _userView;
        IActorRef _userContactsView;
        IActorRef _userChatView;
        IActorRef _userBadgeView;
        IActorRef _region;

        public override string PersistenceId { get { return Self.Path.Name; } }

        public User()
        {
            Context.Become(Unregistered);
            RecoverAny(UpdateState);

            Guid userId = Guid.Parse(Self.Path.Name);

            _userView = Context.ActorOf(Props.Create(() => new UserView()));
            _userContactsView = Context.ActorOf(Props.Create(() => new UserContactsView()));
            _userChatView = Context.ActorOf(Props.Create(() => new UserChatView(userId)));
            _userBadgeView = Context.ActorOf(Props.Create(() => new UserBadgeView(userId)));

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

        void Unregistered(object message)
        {
            message.Match()
                .With<RegisterUserCommand>(register =>
                {
                    var userRegistered = new UserRegisteredEvent(register.Id, register.Login, register.UserName);
                    Persist(userRegistered, UpdateState);
                });
        }

        void Initialized(object message)
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

                        var @event = new SubscribedToUserEvent(cmd.UserId, cmd.ContactUserId, contactUser.Login, contactUser.UserName);
                        Persist(@event, UpdateState);
                    }
                })
                .With<GetUserById>(x =>
                {
                    Sender.Tell(new GetUserByIdResult(State.Id, State.Login, State.UserName));
                })
                .With<ChatCreatedEvent>(x =>
                {
                    ResendToViews(x);
                })
                .With<ChatMessageAddedEvent>(x =>
                {
                    ResendToViews(x);
                })
                .With<MarkChatMessagesReadCommand>(x =>
                {
                    var @event = new ChatMessagesMarkedReadEvent(x.UserId, x.ChatId);
                    Persist(@event, UpdateState);
                });
        }

        void UpdateState(object e)
        {
            e.Match()
            .With<UserRegisteredEvent>(@event =>
            {
                State = new UserEntity(@event.Id, @event.Login, @event.UserName);
                if (!IsRecovering)
                {
                    Context.System.EventStream.Publish(@event);
                    ResendToViews(@event);
                }
                Context.Become(Initialized);
                
            })
            .With<UserNameEmailChangedEvent>(@event =>
            {
                State.UserName = @event.Name;
                if (!IsRecovering)
                {
                    Context.System.EventStream.Publish(@event);
                    ResendToViews(@event);
                }
            })
            .With<SubscribedToUserEvent>(@event =>
            {
                State.SubscribedToList.Add(@event.ContactUserId);
                
                if (!IsRecovering)
                {
                    Context.System.EventStream.Publish(@event);
                    ResendToViews(@event);
                }
            })
            .With<ChatMessagesMarkedReadEvent>(@event =>
            {
                if (!IsRecovering)
                {
                    ResendToViews(@event);
                }
            });
        }
     
        void ResendToViews(object @event)
        {
            _userChatView.Tell(@event);
            _userContactsView.Tell(@event);
            _userView.Tell(@event);
            _userBadgeView.Tell(@event);
        }
    }
}


