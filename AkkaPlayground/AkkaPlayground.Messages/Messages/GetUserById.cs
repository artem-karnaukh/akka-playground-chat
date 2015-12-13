using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetUserById
    {
        public Guid UserId { get; set; }

        public GetUserById(Guid userId)
        {
            UserId = userId;
        }
    }
}
