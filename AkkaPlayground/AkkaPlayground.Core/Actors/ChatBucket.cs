using Akka.Actor;
using AkkaPlayground.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Actors
{
    public class ChatBucket : ReceiveActor
    {
        public ChatBucket()
        {
            Become(Initialized);
        }

        private void Initialized()
        {
            Receive<CreateChatCommand>(x =>
            {
                Forward(x.Id, x);
            });

            Receive<AddMessageToChat>(x =>
            {
                Forward(x.ChatId, x);
            });
        }

        private void Forward(Guid id, object message)
        {
            IActorRef child = Context.Child(id.ToString());
            if (child.Equals(ActorRefs.Nobody))
            {
                child = Context.ActorOf(Props.Create(() => new Chat(id)), id.ToString());
            }
            child.Forward(message);
        }

    }
}
