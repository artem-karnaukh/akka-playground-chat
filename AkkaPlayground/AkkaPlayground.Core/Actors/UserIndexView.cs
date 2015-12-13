using Akka.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using AkkaPlayground.Messages.Events;

namespace AkkaPlayground.Core.Actors
{
    public class UserIndexView : PersistentView
    {
        public override string ViewId
        {
            get { return "user-index-view";  }
        }

        public override string PersistenceId
        {
            get { return "user-index"; }
        }

        private int i = 0;

        public UserIndexView()
        {

        }

        protected override bool Receive(object message)
        {
            return message.Match()
                .With<UserRegisteredEvent>(register =>
                {
                    i++;
                })
                .With<UserNameEmailChangedEvent>(changeUser =>
                {
                    i++;
                })
                .With<RecoveryFailure>(fail =>
                {
                    
                })
                .WasHandled;
        }

        
    }
}
