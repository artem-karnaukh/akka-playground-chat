using AkkaPlayground.Messages.Entities;
using AkkaPlayground.Projections;
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

        public ChatParticipant Creator { get; private set; }

        public List<ChatParticipant> Participants { get; private set; }

        public ChatCreatedEvent(Guid id, ChatParticipant creator, List<ChatParticipant> participants)
        {
            Id = id;
            Creator = creator;
            Participants = participants;
        }
    }
}
