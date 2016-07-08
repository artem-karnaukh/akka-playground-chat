using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Entities
{
    public class ChatParticipant
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string UserName { get; set; }

        public ChatParticipant(Guid id, string login, string userName)
        {
            Id = id;
            Login = login;
            UserName = userName;
        }
    }
}
