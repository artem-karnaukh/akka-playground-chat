//using Akka.Persistence;
//using AkkaPlayground.Messages.Events;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Akka;
//using Akka.Actor;
//using AkkaPlayground.Messages.Messages;
//using AkkaPlayground.Messages.Commands;
//using AkkaPlayground.Core.Entities;

//namespace AkkaPlayground.Core.Actors
//{
//    public class ChatHistoryView : PersistentView
//    {
//        private Guid _chatId;

//        public override string ViewId
//        {
//            get { return "chat-history-view-" + _chatId.ToString(); }
//        }

//        public override string PersistenceId
//        {
//            get { return _chatId.ToString(); }
//        }

//        public List<ChatHistoryReadModel> Messages { get; set; }

//        public ChatHistoryView(Guid chatId)
//        {
//            _chatId = chatId;
//            Messages = new List<ChatHistoryReadModel>();
//        }

//        protected override bool Receive(object message)
//        {
//            return message.Match()
//                .With<ChatMessageAddedEvent>(async x =>
//                {
//                    IActorRef actorRef = Context.System.ActorSelection("user/user-index").ResolveOne(TimeSpan.FromSeconds(10)).Result;
//                    var userFound = await actorRef.Ask<UserFound>(new GetUserById(x.Author));

//                    ChatHistoryReadModel messageReadModel = new ChatHistoryReadModel()
//                    {
//                        Date = x.Date,
//                        Message = x.Message,
//                        UserId = x.Author,
//                        UserName = userFound.Name
//                    };

//                    Messages.Add(messageReadModel);
//                })

//            .WasHandled;
//        }

//        private bool IsRecovering = false;
//        protected override bool AroundReceive(Receive receive, object message)
//        {
//            if (message is Recover)
//            {
//                IsRecovering = true;
//            }

//            if (message is ReplayMessagesSuccess)
//            {
//                IsRecovering = false;
//            }

//            return base.AroundReceive(receive, message);
//        }
//    }
//}
