using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class ChatMessagesMarkedReadEvent
    {
        public Guid UserId { get; set; }

        public Guid ChatId { get; set; }

        public ChatMessagesMarkedReadEvent(Guid userId, Guid chatId)
        {
            UserId = userId;
            ChatId = chatId;
        }
    }
}
