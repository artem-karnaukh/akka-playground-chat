using Akka.Actor;
using Akka.Persistence;
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
            ClusterSystem = ActorSystem.Create("AkkaCluster");
            //var userIndexActor = ClusterSystem.ActorOf(Props.Create(() => new UserIndex()), "user-index");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            ClusterSystem.Shutdown();
            return true;
        }
    }
}
