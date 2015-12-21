using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Entities
{
    public class ChatEntity
    {
        public Guid Id { get; set; }

        public List<Guid> Participants { get; set; }

        public List<ChatLogEntity> Log { get; set; }

        public ChatEntity(Guid id)
        {
            Id = id;
            Participants = new List<Guid>();
            Log = new List<ChatLogEntity>();
        }
    }
}
