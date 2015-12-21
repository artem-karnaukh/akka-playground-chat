using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetUserChats : IConsistentHashable
    {
        public Guid UserId { get; protected set; }

        public object ConsistentHashKey
        {
            get { return UserId; }
        }

        public GetUserChats(Guid userId)
        {
            UserId = userId;
        }

    }
}
