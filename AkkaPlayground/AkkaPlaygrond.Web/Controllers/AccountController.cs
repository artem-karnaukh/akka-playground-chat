using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AkkaPlaygrond.Web.Models;
using AkkaPlaygrond.Web.Actors;
using AkkaPlayground.Messages.Commands;
using Akka.Actor;

namespace AkkaPlaygrond.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
    }
}
