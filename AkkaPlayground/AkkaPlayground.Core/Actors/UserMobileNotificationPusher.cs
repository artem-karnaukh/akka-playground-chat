using Akka.Actor;
using AkkaPlayground.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Actors
{
    public class UserMobileNotificationPusher: ReceiveActor
    {
        public UserMobileNotificationPusher()
        {
            Initialized();
        }

        private void Initialized()
        {
            Receive<ChatMessageAddedEvent>(mes => 
            {
                Console.WriteLine("gonna push to mobile devices the message: " + mes.Message + " from the chat: " + mes.Author);
            });
        }
    }
}
