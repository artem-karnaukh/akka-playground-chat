using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class UserNotFound
    {
        public string Login { get; private set; }

        public UserNotFound(string login)
        {
            Login = login;
        }
    }
}
