using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Entities
{
    public class UserChatEntity
    {
        public Guid ChatId { get; set; }

        public List<Guid> Participants { get; set; }
    }
}
