using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetUserSubscribedToList : IConsistentHashable
    {
        public Guid Id { get; private set; }

        public GetUserSubscribedToList(Guid id)
        {
            Id = id;
        }

        public object ConsistentHashKey
        {
            get { return Id; }
        }
    }
}
