using System.Web.Mvc;


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
