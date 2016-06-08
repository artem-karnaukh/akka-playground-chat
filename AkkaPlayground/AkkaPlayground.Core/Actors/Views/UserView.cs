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
    public class UserView : PersistentView
    {
        private MongoContext _mongoContext;
        private string _userPersistanceId;

        public override string ViewId
        {
            get { return "users-view-"+ _userPersistanceId; }
        }
        
        public override string PersistenceId
        {
            get { return _userPersistanceId; }
        }


        public UserView(string userPersistanceId)
        {
            _userPersistanceId = userPersistanceId;
            _mongoContext = new MongoContext();
        }


        protected override bool Receive(object @event)
        {
            return @event.Match()
                .With<UserRegisteredEvent>(x =>
                {
                    UserProjection userProjection = new UserProjection() { Id = x.Id, Login = x.Login, Email= x.Email };
                    _mongoContext.Users().InsertOne(userProjection);
                })
                .With<UserNameEmailChangedEvent>(x =>
                {

                })
                .WasHandled;
        }
        
    }
}
