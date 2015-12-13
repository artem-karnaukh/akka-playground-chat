using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Commands
{
    public class AddMessageToChat : IConsistentHashable
    {
        public Guid ChatId { get; protected set; }

        public Guid Author { get; protected set; }
        
        public string Message { get; protected set; }

        public AddMessageToChat(Guid chatId, Guid author, string message)
        {
            ChatId = chatId;
            Author = author;
            Message = message;
        }

        public object ConsistentHashKey
        {
            get { return ChatId; }
        }
    }
}
