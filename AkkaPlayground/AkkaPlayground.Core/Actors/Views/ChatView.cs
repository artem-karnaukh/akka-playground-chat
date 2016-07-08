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
    public class ChatView : ReceiveActor
    {
        private IFirebaseClient client;

        public ChatView()
        {

            IFirebaseConfig config = new FirebaseConfig
            {
                BasePath = "https://akkaplayground.firebaseio.com/"
            };
            client = new FirebaseClient(config);

            Ready();
        }

        private void Ready()
        {
            Receive<ChatCreatedEvent>(@event =>
            {
                var chat = new ChatDto() { ChatId = @event.Id };
                chat.Participants = @event.Participants
                    .Select(x => new ChatParticipantDto() { Id = x.Id, Login = x.Login }).ToList();

                string path = "chats/" + @event.Id;
                PushResponse response = client.Push(path, chat);
            });

            Receive<ChatMessageAddedEvent>(@event =>
            {
                string path = String.Format("chats/{0}/{1}", @event.ChatId, "messages");
                ChatMessageDto messageDto = new ChatMessageDto()
                {
                    MessageId = @event.MessageId,
                    Date = @event.Date,
                    Message = @event.Message,
                    UserId = @event.Author.Id,
                    UserName = @event.Author.Login
                };
                PushResponse response = client.Push(path, messageDto);
            });
        }

        protected override void PostStop()
        {
            base.PostStop();
        }
    }
}
