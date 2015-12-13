using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkkaPlaygrond.Web.Actors
{
    public static class SystemActors
    {
        public static IActorRef SignalRActor = ActorRefs.Nobody;
    }
}