using AkkaPlayground.Messages.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class ChatMessageAddedEvent
    {
        public Guid ChatId { get; private set; }

        public DateTime Date { get; private  set; }

        public Guid MessageId { get; private set; }

        public string Message { get; private set; }

        public ChatParticipant Author { get; private set; }

        public List<ChatParticipant> ChatParticipants { get; private set; }

        public ChatMessageAddedEvent(Guid messageId, Guid chatId, DateTime date, string message, ChatParticipant author)
        {
            MessageId = messageId;
            ChatId = chatId;
            Date = date;
            Message = message;
            Author = author;
        }

    }
}
