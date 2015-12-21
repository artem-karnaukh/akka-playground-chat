using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.ReadModels
{
    public class UserChatReadModel
    {
        public Guid ChatId { get; set; }
        
        public string LastMessage { get; set; }

        public DateTime? LastMessageDate { get; set; }

        public List<UserChatParticipantReadModel> Participants { get; set; }

        public UserChatReadModel()
        {
            Participants = new List<UserChatParticipantReadModel>();
        }
    }
}
