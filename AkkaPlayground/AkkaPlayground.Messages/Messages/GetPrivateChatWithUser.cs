using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetPrivateChatWithUser : IConsistentHashable
    {
        public Guid UserId { get; private set; }

        public Guid TargetUserId { get; private set; }

        public GetPrivateChatWithUser(Guid userId, Guid targetUserId)
        {
            UserId = userId;
            TargetUserId = targetUserId;
        }

        public object ConsistentHashKey
        {
            get { return UserId; }
        }
    }
}
