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
        public Guid Id { get; private set; }
        public string Login { get; private set; }
        public string UserName { get; private set; }

        public RegisterUserCommand(Guid id, string login, string userName)
        {
            Id = id;
            Login = login;
            UserName = userName;
        }
    }
}
