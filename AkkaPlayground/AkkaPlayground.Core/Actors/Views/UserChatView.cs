using Akka.Actor;
using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Events;
using AkkaPlayground.Projections;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Actors.Views
{
    public class UserChatView : ReceiveActor
    {
        private IFirebaseClient client;
        private Guid _userId;

        public UserChatView(Guid userId)
        { 
            IFirebaseConfig config = new FirebaseConfig
            {
                BasePath = "https://akkaplayground.firebaseio.com/"
            };
            client = new FirebaseClient(config);
            _userId = userId;

            Ready();
        }

        private void Ready()
        {
            Receive<ChatCreatedEvent>(@event =>
            {
                string path = "user-chats/" + _userId;
                string lastMessage = String.Format("{0} created a chat with you", @event.Creator.Login);
                if (@event.Creator.Id == _userId)
                {
                    lastMessage = "You created a chat";
                }

                var userChat = new UserChatDto()
                {
                    ChatId = @event.Id,
                    UserId = _userId,
                    LastMessage = lastMessage,
                    LastMessageAuthor = @event.Creator.Login,
                    LastMessageDate = DateTime.UtcNow
                };

                userChat.Participants = @event.Participants
                    .Select(x => new ChatParticipantDto() { Id = x.Id, Login = x.Login }).ToList();
                string participantNames = string.Join(", ",
                    userChat.Participants.Where(x => x.Id != _userId).Select(x => x.Login));
                userChat.Name = participantNames;

                client.Push(path, userChat);
            });

            Receive<ChatMessageAddedEvent>(@event =>
            {
                string path = "user-chats/" + _userId;
                FirebaseResponse response = client.Get(path);
                Dictionary<string, UserChatDto> chats = response.ResultAs<Dictionary<string, UserChatDto>>();

                if (chats != null && chats.Any(x => x.Value.ChatId == @event.ChatId))
                {
                    var chat = chats.First(x => x.Value.ChatId == @event.ChatId);
                    
                    string userChatPathLastMessage = String.Format("user-chats/{0}/{1}/LastMessage", _userId, chat.Key);
                    client.Set(userChatPathLastMessage, @event.Message);

                    string userChatPathLastMessageAuthor = String.Format("user-chats/{0}/{1}/LastMessageAuthor", _userId, chat.Key);
                    client.Set(userChatPathLastMessageAuthor, @event.Author.Login);

                    string userChatPathLastMessageDate = String.Format("user-chats/{0}/{1}/LastMessageDate", _userId, chat.Key);
                    client.Set(userChatPathLastMessageDate, @event.Date);
                }


            });
        }

        protected override void PostStop()
        {
            base.PostStop();
        }
    }
}
