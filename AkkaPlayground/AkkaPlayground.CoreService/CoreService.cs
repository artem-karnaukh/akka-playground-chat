using Akka.Actor;
using Akka.Persistence.Sqlite;
using AkkaPlayground.Core.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace AkkaPlayground.CoreService
{
    public class CoreService : ServiceControl
    {
        protected ActorSystem ClusterSystem;

        public bool Start(HostControl hostControl)
        {
            ClusterSystem = ActorSystem.Create("akka-persistance");
            //var userIndexActor = ClusterSystem.ActorOf(Props.Create(() => new UserIndex()), "user-index");
            //var persistance = SqlitePersistence.Get(ClusterSystem);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            ClusterSystem.Shutdown();
            return true;
        }
    }
}
