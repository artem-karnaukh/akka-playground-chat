using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class UserPrivateChatReult
    {
        public Guid? ChatId { get; private set; }

        public UserPrivateChatReult(Guid? chatId)
        {
            ChatId = chatId;
        }
    }
}
