using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class UserAddedToChatEvent
    {
        public Guid ChatId { get; private set; }

        public List<Guid> Participants { get; set; }

        public UserAddedToChatEvent(Guid chatId, List<Guid> participants)
        {
            ChatId = chatId;
            Participants = participants;
        }
    }
}
