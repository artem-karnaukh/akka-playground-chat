using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.ReadModels
{
    public class UserChatParticipantReadModel
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

    }
}
