using Akka.Actor;
using AkkaPlaygrond.Web.Actors;
using AkkaPlaygrond.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AkkaPlaygrond.Web.Areas.api.Controllers
{
    public class ChatApiController : Controller
    {

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