using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Entities
{
    public class ChatHistoryReadModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
