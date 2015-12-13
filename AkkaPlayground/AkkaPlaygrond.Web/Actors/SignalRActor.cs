using Akka.Actor;
using AkkaPlaygrond.Web.Models;
using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Events;
using AkkaPlayground.Messages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkkaPlaygrond.Web.Actors
{
    public class SignalRActor : ReceiveActor
    {
        protected readonly IActorRef _userBuckerRouter;

        protected readonly IActorRef _chatBucketRouter;

        protected readonly IActorRef _userIndex;

        public SignalRActor(IActorRef userBucketRouter, IActorRef chatBucketRouter, IActorRef userIndex)
        {
            _userBuckerRouter = userBucketRouter;
            _chatBucketRouter = chatBucketRouter;
            _userIndex = userIndex;

            Receive();
        }

        private void Receive()
        {
            Receive<RegisterModel>(register =>
            {
                _userBuckerRouter.Tell(new RegisterUserCommand(register.UserId, register.UserName, register.Email));
            });


            Receive<GetUsersBySearchString>(mes =>
            {
                _userIndex.Ask<UserSearchResult>(mes).PipeTo(Sender, Self);
            });

            Receive<GetUserByLogin>(mes =>
            {
                _userIndex.Ask(mes).PipeTo(Sender, Self);
            });

            Receive<SubscribeToUserCommand>(mes =>
            {
                _userBuckerRouter.Ask<SubscribedToUserEvent>(mes).PipeTo(Sender, Self);
            });

            Receive<GetUserSubscribedToList>(mes =>
            {
                _userBuckerRouter.Ask<SubscribedToListResult>(mes).PipeTo(Sender, Self);
            });

            Receive<GetPrivateChatWithUser>(mes =>
            {
                _userBuckerRouter.Ask<UserPrivateChatReult>(mes).PipeTo(Sender, Self);
            });

            Receive<CreateChatCommand>(mes =>
            {
                _chatBucketRouter.Ask<ChatCreatedEvent>(mes).PipeTo(Sender, Self);
            });

            Receive<AddMessageToChat>(cmd =>
            {
                _chatBucketRouter.Tell(cmd);
            });

            Receive<UserRegisteredEvent>(evt =>
            {
                SignalREventPusher pusher = new SignalREventPusher();
                pusher.PlayerJoined(evt.Id, evt.Name, evt.Email);
            });

            Receive<ChatMessageAddedEvent>(evt =>
            {
                SignalREventPusher pusher = new SignalREventPusher();
                pusher.ChatMessageAdded(evt);
            });

        }
    }
}