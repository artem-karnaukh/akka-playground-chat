using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class UserAddedToFollowersEvent
    {
        public Guid UserId { get; private set; }

        public Guid TargetUserId { get; private set; }

        public UserAddedToFollowersEvent(Guid userId, Guid userToAddId)
        {
            UserId = userId;
            TargetUserId = userToAddId;
        }

    }
}
