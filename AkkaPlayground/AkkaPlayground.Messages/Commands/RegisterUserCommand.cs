using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Commands
{
    public class RegisterUserCommand
    {
        public readonly Guid Id;
        public readonly string Login;
        public readonly string Email;

        public RegisterUserCommand(Guid id, string login, string email)
        {
            Id = id;
            Login = login;
            Email = email;
        }
    }
}
