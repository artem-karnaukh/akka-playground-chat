using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class UserRegisteredEvent 
    {
        public Guid Id { get; private set; }
        public string Login { get; private set; }
        public string UserName { get; private set; }

        public UserRegisteredEvent(Guid id, string login, string userName)
        {
            Id = id;
            Login = login;
            UserName = userName;
        }

    }
}
