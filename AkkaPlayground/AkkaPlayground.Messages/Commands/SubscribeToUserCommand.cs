using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Commands
{
    public class SubscribeToUserCommand 
    {
        public Guid UserId { get; private set; }

        public Guid ContactUserId { get; private set; }

        public SubscribeToUserCommand(Guid userId, Guid userToAddId)
        {
            UserId = userId;
            ContactUserId = userToAddId;
        }
    }
}
