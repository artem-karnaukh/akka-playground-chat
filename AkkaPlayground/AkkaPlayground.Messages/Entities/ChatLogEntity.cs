using AkkaPlayground.Messages.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Entities
{
    public class ChatLogEntity
    {
        public DateTime Date { get; set; }

        public Guid MessageId { get; set; }

        public string Message { get; set; }

        public ChatParticipant Author { get; set; }

        public ChatLogEntity(Guid messageId, ChatParticipant author, string message, DateTime date)
        {
            MessageId = messageId;
            Author = author;
            Message = message;
            Date = date;
        }
    }
}
