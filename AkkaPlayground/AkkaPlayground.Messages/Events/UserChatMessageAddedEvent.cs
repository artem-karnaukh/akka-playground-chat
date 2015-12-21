using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class UserChatMessageAddedEvent
    {
        public Guid ChatId { get; set; }

        public DateTime Date { get; set; }

        public string Message { get; set; }

        public Guid Author { get; set; }

        public UserChatMessageAddedEvent(Guid chatId, Guid author, string message, DateTime date)
        {
            ChatId = chatId;
            Author = author;
            Message = message;
            Date = date;
        }
    }
}
