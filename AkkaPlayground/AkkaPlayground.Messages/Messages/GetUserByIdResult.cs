using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetUserByIdResult
    {
        public string Login { get; private set; }

        public string Name { get; private set; }

        public GetUserByIdResult(string login, string name)
        {
            Login = login;
            Name = name;
        }

    }
}
