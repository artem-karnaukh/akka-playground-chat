using Akka.Actor;
using Akka.Persistence.SqlServer;
using Akka.Routing;
using AkkaPlaygrond.Web.Actors;
using AkkaPlayground.Core.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AkkaPlaygrond.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected static ActorSystem ActorSystem;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ActorSystem = ActorSystem.Create("AkkaCluster");
            SqlServerPersistence.Get(ActorSystem);

            SystemActors.SignalRActor =
                ActorSystem.ActorOf(Props.Create(() => new SignalRActor()), "signalr");
        }

        protected void Application_End()
        {
            ActorSystem.Shutdown();
        }
    }
}