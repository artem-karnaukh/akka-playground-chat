using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Events
{
    public class UserNameEmailChangedEvent
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Email;

        public UserNameEmailChangedEvent(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
