using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Projections
{
    public class ChatDto
    {
        public Guid ChatId { get; set; }

        public List<ChatParticipantDto> Participants { get; set; }

        public List<ChatMessageDto> Messages { get; set; }
    }
}
