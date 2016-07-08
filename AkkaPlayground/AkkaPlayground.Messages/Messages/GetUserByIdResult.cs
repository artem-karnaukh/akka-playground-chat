using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetUserByIdResult
    {
        public Guid Id { get; set; }

        public string Login { get; private set; }

        public string UserName { get; private set; }

        public GetUserByIdResult(Guid id, string login, string name)
        {
            Id = id;
            Login = login;
            UserName = name;
        }

    }
}
