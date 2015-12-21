using AkkaPlayground.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class ChatHistoryResult
    {
        public Guid ChatId { get; protected set; }

        public List<ChatLogEntity> Log { get; protected set; }

        public ChatHistoryResult(Guid chatId, List<ChatLogEntity> log)
        {
            ChatId = chatId;
            Log = log;
        }
    }
}
