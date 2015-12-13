using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetUserContacts
    {
        public Guid UserId { get; private set; }

        public GetUserContacts(Guid userId)
        {
            UserId = userId;
        }
    }
}
