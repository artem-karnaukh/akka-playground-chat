using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Commands
{
    public class MarkChatMessagesReadCommand
    {
        public Guid UserId { get; private set; }

        public Guid ChatId { get; private set; }

        public MarkChatMessagesReadCommand(Guid userId, Guid chatId)
        {
            UserId = userId;
            ChatId = chatId;
        }
    }
}
