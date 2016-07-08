using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Sharding;
using AkkaPlaygrond.Web.Models;
using AkkaPlayground.Core.Actors;
using AkkaPlayground.Messages;
using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Events;
using AkkaPlayground.Messages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Akka.Cluster.ClusterEvent;

namespace AkkaPlaygrond.Web.Actors
{
    public class SignalRActor : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get;set; }

        IActorRef UserRegion { get; set; }

        IActorRef ChatRegion { get; set; }

        Cluster Cluster { get; set; }

        SignalREventPusher SignalrPusher { get; set; }

        public SignalRActor()
        {
            SignalrPusher = new SignalREventPusher();

            Cluster = Akka.Cluster.Cluster.Get(Context.System);
            Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents, new[] { typeof(ClusterEvent.IMemberEvent),
                typeof(ClusterEvent.UnreachableMember) });
            HeatingUp();
        }

        void HeatingUp()
        {
            Receive<object>(x =>
            {
                if (x is MemberUp)
                {
                    Become(Ready);
                    UserRegion = ClusterSharding.Get(Context.System).Start(
                        typeName: typeof(User).Name,
                        entityProps: Props.Create<User>(),
                        settings: ClusterShardingSettings.Create(Context.System),
                        messageExtractor: new MessageExtractor(10)
                        );

                    ChatRegion = ClusterSharding.Get(Context.System).Start(
                        typeName: typeof(Chat).Name,
                        entityProps: Props.Create<Chat>(),
                        settings: ClusterShardingSettings.Create(Context.System),
                        messageExtractor: new MessageExtractor(10)
                        );

                    Stash.UnstashAll();
                }
                else
                {
                    Stash.Stash();
                }
            });
        }

        void Ready()
        {

            Context.System.EventStream.Subscribe(Self, typeof(UserRegisteredEvent));
            Context.System.EventStream.Subscribe(Self, typeof(SubscribedToUserEvent));

            Receive<RegisterModel>(register =>
            {
                var command = new RegisterUserCommand(register.UserId, register.Login, register.UserName);
                var envelope = new ShardEnvelope(command.Id.ToString(), command);
                UserRegion.Tell(envelope);
            });

            Receive<CreateChatModel>(x =>
            {
                var command = new CreateChatCommand(x.ChatId, x.UserId, new List<Guid> { x.UserId, x.TargetUserId });
                var envelope = new ShardEnvelope(command.Id.ToString(), command);
                ChatRegion.Tell(envelope);
            });

            Receive<UserRegisteredEvent>(evt =>
            {
                SignalrPusher.PlayerJoined(evt.Id, evt.Login, evt.UserName);
            });

            Receive<SubscribeToUserCommand>(mes =>
            {
                var envelope = new ShardEnvelope(mes.UserId.ToString(), mes);
                UserRegion.Tell(envelope);
            });

            Receive<SubscribedToUserEvent>(mes =>
            {
                SignalrPusher.UserAddedToContactList(mes.UserId, mes.ContactUserId, mes.ContactName);
            });

            Receive<AddMessageToChatCommand>(cmd =>
            {
                var envelope = new ShardEnvelope(cmd.ChatId.ToString(), cmd);
                ChatRegion.Tell(envelope);
            });

            Receive<MarkChatMessagesReadCommand>(cmd =>
            {
                var envelope = new ShardEnvelope(cmd.UserId.ToString(), cmd);
                UserRegion.Tell(envelope);
            });

            

        }
    }
}