using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetUserByEmail 
    {
        public string Email { get; private set; }

        public GetUserByEmail(string email)
        {
            Email = email;
        }
    }
}
