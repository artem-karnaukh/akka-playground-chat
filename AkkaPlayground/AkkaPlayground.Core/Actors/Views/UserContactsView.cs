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

namespace AkkaPlayground.Core.Actors.Views
{
    public class UserContactsView : PersistentView
    {
        private MongoContext _mongoContext;
        private string _userPersistanceId;

        public override string ViewId
        {
            get { return "user-contact-list-view-" + _userPersistanceId; }
        }

        public override string PersistenceId
        {
            get { return _userPersistanceId; }
        }

        public UserContactsView(string userPersistanceId)
        {
            _userPersistanceId = userPersistanceId;
            _mongoContext = new MongoContext();
        }

        protected override bool Receive(object @event)
        {
            return @event.Match()
                .With<SubscribedToUserEvent>(x =>
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
                })
                .WasHandled;
        }
    }
}
