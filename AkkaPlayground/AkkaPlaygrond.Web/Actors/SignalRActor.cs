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

        private IActorRef UserRegion { get; set; }

        private IActorRef ChatRegion { get; set; }

        private Cluster Cluster { get; set; }

        private SignalREventPusher SignalrPusher { get; set; }

        public SignalRActor()
        {
            SignalrPusher = new SignalREventPusher();

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

        private void Ready()
        {

            Context.System.EventStream.Subscribe(Self, typeof(UserRegisteredEvent));
            Context.System.EventStream.Subscribe(Self, typeof(SubscribedToUserEvent));

            Receive<RegisterModel>(register =>
            {
                var command = new RegisterUserCommand(register.UserId, register.UserName, register.Email);
                var envelope = new ShardEnvelope(command.Id.ToString(), command);
                UserRegion.Tell(envelope);
            });

            Receive<CreateChatModel>(x =>
            {
                var command = new CreateChatCommand(x.ChatId, new List<Guid> { x.UserId, x.TargetUserId });
                var envelope = new ShardEnvelope(command.Id.ToString(), command);
                ChatRegion.Tell(envelope);
            });
            

            Receive<UserRegisteredEvent>(evt =>
            {
                SignalrPusher.PlayerJoined(evt.Id, evt.Login, evt.Email);
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