using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Projections
{
    public class ChatMessageDto
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public Guid MessageId { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
