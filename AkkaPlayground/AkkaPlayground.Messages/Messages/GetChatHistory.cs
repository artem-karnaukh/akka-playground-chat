using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetChatHistory : IConsistentHashable
    {
        public Guid ChatId { get; protected set; }

        public int? Skip { get; protected set; }

        public int? Take { get; protected set; }

        public object ConsistentHashKey
        {
            get { return ChatId; }
        }

        public GetChatHistory(Guid chatId, int? skip = null, int? take = null)
        {
            ChatId = chatId;
            Skip = skip;
            Take = take;
        }
    }
}
