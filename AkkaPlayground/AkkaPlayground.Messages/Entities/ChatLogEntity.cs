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

        public string Message { get; set; }

        public Guid Author { get; set; }

        public ChatLogEntity(Guid author, string message, DateTime date)
        {
            Author = author;
            Message = message;
            Date = date;
        }
    }
}
