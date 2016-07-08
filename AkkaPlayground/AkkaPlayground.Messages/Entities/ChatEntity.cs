using AkkaPlayground.Messages.Entities;
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

        public List<ChatParticipant> Participants { get; set; }

        public List<ChatLogEntity> Log { get; set; }

        public ChatEntity(Guid id)
        {
            Id = id;
            Participants = new List<ChatParticipant>();
            Log = new List<ChatLogEntity>();
        }
    }
}
