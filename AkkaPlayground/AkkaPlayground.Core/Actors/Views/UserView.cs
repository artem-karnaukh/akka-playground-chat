using Akka.Persistence;
using AkkaPlayground.Core.Entities;
using AkkaPlayground.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;
using AkkaPlayground.Messages.Messages;
using AkkaPlayground.Messages.ReadModels;
using AkkaPlayground.Core.Data;
using AkkaPlayground.Projections;

namespace AkkaPlayground.Core.Actors.Views
{
    public class UserView : ReceiveActor
    {
        private MongoContext _mongoContext;

        public UserView()
        {
            _mongoContext = new MongoContext();
            Ready();
        }

        private void Ready()
        {
            Receive<UserRegisteredEvent>(x =>
            {
                UserProjection userProjection = new UserProjection() { Id = x.Id, Login = x.Login, Email = x.Email };
                _mongoContext.Users().InsertOne(userProjection);
            });
            Receive<UserNameEmailChangedEvent>(x =>
            {

            });
        }
        
    }
}
