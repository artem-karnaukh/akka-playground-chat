using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.RemoteTarget
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var system = ActorSystem.Create("DeployTarget"))
            {
                Console.ReadLine();
            }
        }
    }
}
