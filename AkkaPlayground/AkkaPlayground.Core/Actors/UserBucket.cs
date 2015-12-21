using Akka.Actor;
using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Events;
using AkkaPlayground.Messages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Actors
{
    public class UserBucket : ReceiveActor
    {
        public UserBucket()
        {
            Become(Initialized);
        }

        private void Initialized()
        {
            Receive<RegisterUserCommand>(x =>
            {
                Forward(x.Id, x);
            });

            Receive<ChangeUserNameEmailCommand>(x =>
            {
                Forward(x.Id, x);
            });

            Receive<SubscribeToUserCommand>(x =>
            {
                Forward(x.UserId, x);
            });

            Receive<GetUserSubscribedToList>(x =>
            {
                Forward(x.Id, x);
            });

            Receive<GetPrivateChatWithUser>(x =>
            {
                Forward(x.UserId, x);
            });

            Receive<GetUserChats>(x =>
            {
                Forward(x.UserId, x);
            });
        }

        private void Forward(Guid id, object message)
        {
            IActorRef child = Context.Child(id.ToString());
            if (child.Equals(ActorRefs.Nobody))
            {
                child = Context.ActorOf(Props.Create(() => new User(id)), id.ToString());
            }
            child.Forward(message);
        }

    }
}
