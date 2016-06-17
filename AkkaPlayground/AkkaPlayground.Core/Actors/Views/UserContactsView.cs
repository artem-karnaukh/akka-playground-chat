using Akka;
using Akka.Persistence;
using AkkaPlayground.Core.Data;
using AkkaPlayground.Messages.Events;
using AkkaPlayground.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AkkaPlayground.Core.Actors.Views
{
    public class UserContactsView : ReceiveActor
    {
        private MongoContext _mongoContext;

        private string Id { get; set; }

        public UserContactsView()
        {
            _mongoContext = new MongoContext();

            Ready();
        }

        private void Ready()
        {
            Receive<SubscribedToUserEvent>(x =>
              {
                  UserContactsProjection userProjection = new UserContactsProjection()
                  {
                      Id = Guid.NewGuid(),
                      UserId = x.UserId,
                      ContactUserId = x.ContactUserId,
                      ContactLogin = x.ContactLogin,
                      ContactName = x.ContactName
                  };
                  _mongoContext.UserContacts().InsertOne(userProjection);
              });
        }
    }
}
