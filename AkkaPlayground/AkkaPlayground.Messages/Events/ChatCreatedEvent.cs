using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class ChatCreatedEvent
    {
        public Guid Id { get; private set; }

        public List<Guid> Participants { get; private set; }

        public ChatCreatedEvent(Guid id, List<Guid> participants)
        {
            Id = id;
            Participants = participants;
        }
    }
}
