using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Commands
{
    public class CreateChatCommand : IConsistentHashable
    {
        public Guid Id { get; private set; }

        public List<Guid> Participants { get; private set; }

        public CreateChatCommand(Guid id, List<Guid> participants)
        {
            Id = id;
            Participants = participants;
        }

        public object ConsistentHashKey
        {
            get { return Id; }
        }
    }
}
