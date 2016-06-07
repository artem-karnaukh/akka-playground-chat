using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class SubscribedToUserEvent 
    {
        public Guid UserId { get; private set; }

        public Guid ContactUserId { get; private set; }

        public string ContactLogin { get; private set; }

        public string ContactName { get; private set; }

        public SubscribedToUserEvent(Guid userId, Guid contactListUserId, string contactLogin, string contactName)
        {
            UserId = userId;
            ContactUserId = contactListUserId;
            ContactLogin = contactLogin;
            ContactName = contactName;
        }
    }
}
