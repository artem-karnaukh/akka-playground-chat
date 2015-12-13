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

        public Guid TargetUserId { get; private set; }

        public SubscribedToUserEvent(Guid userId, Guid contactListUserId)
        {
            UserId = userId;
            TargetUserId = contactListUserId;
        }
    }
}
