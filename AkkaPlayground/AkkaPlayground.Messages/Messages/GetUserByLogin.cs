using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetUserByLogin
    {
        public string Login { get; private set; }

        public GetUserByLogin(string login)
        {
            Login = login;
        }
    }
}
