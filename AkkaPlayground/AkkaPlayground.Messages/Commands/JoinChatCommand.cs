using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Commands
{
    public class JoinChatCommand
    {
        public Guid ChatId { get; private set; }

        public Guid UserId { get; private set; }

        public JoinChatCommand(Guid chatId, Guid userId)
        {
            ChatId = chatId;
            
            UserId = userId;
        }
    }
}
