using Akka.Actor;
using AkkaPlayground.Core.Data;
using AkkaPlayground.Messages.Events;
using AkkaPlayground.Projections;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using FireSharp.Response;

namespace AkkaPlayground.Core.Actors.Views
{
    public class UserBadgeView : ReceiveActor
    {
        private IFirebaseClient _fireBaseClient;
        private MongoContext _mongoContext;
        private Guid _userId;

        public UserBadgeView(Guid userId)
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                BasePath = "https://akkaplayground.firebaseio.com/"
            };
            _fireBaseClient = new FirebaseClient(config);
            _mongoContext = new MongoContext();

            _userId = userId;

            Ready();
        }

        private void Ready()
        {
            Receive((Action<ChatMessageAddedEvent>)(@event =>
            {
                if (@event.Author.Id == _userId)
                {
                    return;
                }

                UserBadgeEntryDto badgeEntry = new UserBadgeEntryDto()
                {
                    Id = Guid.NewGuid(),
                    MessageId = @event.MessageId,
                    ChatId = @event.ChatId,
                    UserId = _userId,
                    Date = @event.Date
                };

                _mongoContext.UserBadgeEntries().InsertOne(badgeEntry);

                SetUserChatBadges(@event.ChatId);
                SetUserChatBadges();
            }));

            Receive<ChatMessagesMarkedReadEvent>(@event =>
            {
                _mongoContext.UserBadgeEntries().DeleteMany(x => x.ChatId == @event.ChatId && x.UserId == @event.UserId);
                SetUserChatBadges(@event.ChatId);
                SetUserChatBadges();
            });
        }

        private void SetUserChatBadges(Guid chatId)
        {
            int unreadCount = _mongoContext.UserBadgeEntries().AsQueryable()
                .Where(x => x.UserId == _userId && x.ChatId == chatId).Count();

            string path = "user-chats/" + _userId;
            FirebaseResponse response = _fireBaseClient.Get(path);
            Dictionary<string, UserChatDto> chats = response.ResultAs<Dictionary<string, UserChatDto>>();

            if (chats != null && chats.Any(x => x.Value.ChatId == chatId))
            {
                var chat = chats.First(x => x.Value.ChatId == chatId);
                string unreadCountPath = String.Format("user-chats/{0}/{1}/UnReadCount", _userId, chat.Key);
                _fireBaseClient.Set(unreadCountPath, unreadCount);
            }
        }

        private void SetUserChatBadges()
        {
            int unreadCount = _mongoContext.UserBadgeEntries().AsQueryable()
                .Where(x => x.UserId == _userId).Count();

            string unreadCountPath = String.Format("user-badges/{0}/UnReadCount", _userId);
            _fireBaseClient.Set(unreadCountPath, unreadCount);

        }

        protected override void PostStop()
        {
            base.PostStop();
        }
    }
}
