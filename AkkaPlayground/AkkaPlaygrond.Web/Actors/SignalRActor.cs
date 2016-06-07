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

        private IActorRef Region { get; set; }

        private Cluster Cluster { get; set; }

        public SignalRActor()
        {
            Cluster = Akka.Cluster.Cluster.Get(Context.System);
            Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents, new[] { typeof(ClusterEvent.IMemberEvent),
                typeof(ClusterEvent.UnreachableMember) });
            HeatingUp();
        }

        private void HeatingUp()
        {
            Receive<object>(x =>
            {
                if (x is MemberUp)
                {
                    Become(Ready);
                    Region = ClusterSharding.Get(Context.System).Start(
                        typeName: typeof(User).Name,
                        entityProps: Props.Create<User>(),
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

        private void Ready()
        {

            Context.System.EventStream.Subscribe(Self, typeof(UserRegisteredEvent));

            Receive<RegisterModel>(register =>
            {
                var command = new RegisterUserCommand(register.UserId, register.UserName, register.Email);
                var envelope = new ShardEnvelope(command.Id.ToString(), command);
                Region.Tell(envelope);
            });

            Receive<UserRegisteredEvent>(evt =>
            {
                SignalREventPusher pusher = new SignalREventPusher();
                pusher.PlayerJoined(evt.Id, evt.Login, evt.Email);
            });

            Receive<SubscribeToUserCommand>(mes =>
            {
                var envelope = new ShardEnvelope(mes.UserId.ToString(), mes);
                Region.Tell(envelope);
            });


            //Receive<GetUserSubscribedToList>(mes =>
            //{
            //    _userBuckerRouter.Ask<SubscribedToListResult>(mes).PipeTo(Sender, Self);
            //});

            //Receive<GetPrivateChatWithUser>(mes =>
            //{
            //    _userBuckerRouter.Ask<UserPrivateChatReult>(mes).PipeTo(Sender, Self);
            //});

            //Receive<CreateChatCommand>(mes =>
            //{
            //    _chatBucketRouter.Ask<ChatCreatedEvent>(mes).PipeTo(Sender, Self);
            //});

            //Receive<AddMessageToChat>(cmd =>
            //{
            //    _chatBucketRouter.Tell(cmd);
            //});

            //Receive<GetChatHistory>(mes =>
            //{
            //    _chatBucketRouter.Ask<ChatHistoryResult>(mes).PipeTo(Sender, Self);
            //});



            //Receive<ChatMessageAddedEvent>(evt =>
            //{
            //    SignalREventPusher pusher = new SignalREventPusher();
            //    pusher.ChatMessageAdded(evt);
            //});

            //Receive<GetUserChats>(mes =>
            //{
            //    _userBuckerRouter.Ask<UserChatsResult>(mes).PipeTo(Sender, Self);
            //});

        }
    }
}