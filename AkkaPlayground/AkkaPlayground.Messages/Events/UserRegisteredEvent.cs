using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class UserRegisteredEvent 
    {
        public readonly Guid Id;
        public readonly string Login;
        public readonly string Email;

        public UserRegisteredEvent(Guid id, string login, string email)
        {
            Id = id;
            Login = login;
            Email = email;
        }

    }
}
