using System.Web;
using System.Web.Optimization;

namespace AkkaPlaygrond.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib")
                .Include("~/Scripts/lib/ionic.bundle.js",
                        "~/Scripts/lib/jquery-1.8.2.js",
                        "~/Scripts/lib/jquery.signalR-2.2.0.js",
                        "~/Scripts/lib/moment.js",
                        "~/Scripts/lib/signalr-hub.js"

                ));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/Scripts/app/app.js",
                        "~/Scripts/app/controllers/userSearchCtrl.js",
                        "~/Scripts/app/controllers/contactsCtrl.js",
                        "~/Scripts/app/controllers/loginCtrl.js",
                        "~/Scripts/app/controllers/registerCtrl.js",
                        "~/Scripts/app/utils/userContext.js",
                        "~/Scripts/app/controllers/chatCtrl.js",
                        "~/Scripts/app/controllers/chatsListCtrl.js",
                        "~/Scripts/app/utils/userHub.js",
                        "~/Scripts/app/services/userSvc.js",
                        "~/Scripts/app/services/chatSvc.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/Site.css", 
                         "~/Content/ionic.css"));
        }
    }
}