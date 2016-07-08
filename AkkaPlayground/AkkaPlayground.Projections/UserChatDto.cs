using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Projections
{
    public class UserChatDto
    {
        public Guid UserId { get; set; }

        public Guid ChatId { get; set; }

        public List<ChatParticipantDto> Participants { get; set; }

        public string Name { get; set; }

        public string LastMessage { get; set; }

        public string LastMessageAuthor { get; set; }

        public DateTime LastMessageDate { get; set; }

        public int UnReadCount { get; set; }

    }
}
