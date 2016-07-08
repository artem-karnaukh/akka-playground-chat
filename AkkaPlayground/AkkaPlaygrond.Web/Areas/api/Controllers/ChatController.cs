using Akka.Actor;
using AkkaPlaygrond.Web.Actors;
using AkkaPlaygrond.Web.Models;
using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Projections;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AkkaPlaygrond.Web.Areas.api.Controllers
{
    public class ChatApiController : Controller
    {
        private IFirebaseClient client;

        public ChatApiController()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                BasePath = "https://akkaplayground.firebaseio.com/"
            };
            client = new FirebaseClient(config);

        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetByUser(Guid userId, Guid targetUserId)
        {
            string path = "user-chats/" + userId;
            FirebaseResponse response = client.Get(path);
            Dictionary<string, UserChatDto> chats = response.ResultAs<Dictionary<string, UserChatDto>>();

            if (chats != null)
            {
                UserChatDto userChat = chats.Select(x => x.Value)
                        .Where(x => x.Participants != null &&
                                x.Participants.Count == 2 &&
                                x.Participants.Any(y => y.Id == targetUserId))
                        .FirstOrDefault();

                return Json(userChat, JsonRequestBehavior.AllowGet);

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult AddMessage(Guid chatId, Guid author, string message)
        {
            SystemActors.SignalRActor.Tell(new AddMessageToChatCommand(chatId, author, message), ActorRefs.Nobody);
            return Json(true);
        }



        [HttpPost]
        [AllowAnonymous]
        public Guid Create(CreateChatModel model)
        {
            Guid chatId = Guid.NewGuid();
            model.ChatId = chatId;
            SystemActors.SignalRActor.Tell(model, ActorRefs.Nobody);
            return chatId;
        }

    }
}