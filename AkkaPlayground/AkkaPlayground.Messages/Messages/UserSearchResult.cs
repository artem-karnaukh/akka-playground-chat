using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class UserSearchResult
    {
        public List<UserFound> Users { get; private set; }

        public UserSearchResult(List<UserFound> userFound)
        {
            Users = userFound;
        }
    }
}
