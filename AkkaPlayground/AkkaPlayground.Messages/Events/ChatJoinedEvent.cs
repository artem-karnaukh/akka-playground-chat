using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class ChatJoinedEvent
    {
        public Guid ChatId { get; protected set; }

        public Guid UserId { get; protected set; }

        public ChatJoinedEvent(Guid chatId, Guid userId)
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
