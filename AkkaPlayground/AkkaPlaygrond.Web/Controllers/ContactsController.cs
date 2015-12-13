using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AkkaPlaygrond.Web.Controllers
{
    public class ContactsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Added()
        {
            return View();
        }

    }
}
