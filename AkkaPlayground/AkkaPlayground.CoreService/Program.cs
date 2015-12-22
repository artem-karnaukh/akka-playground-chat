using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace AkkaPlayground.CoreService
{
    class Program
    {
        static int  Main(string[] args)
        {
            return (int)HostFactory.Run(x =>
            {
                x.SetServiceName("AkkaPlaygroundCore");
                x.SetDisplayName("Akka.NET Playground Core");
                x.SetDescription("Akka.NET Playground Cluster.");

                x.UseAssemblyInfoForServiceInfo();
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.Service<CoreService>();
                x.EnableServiceRecovery(r => r.RestartService(1));
            });
        }
    }
}
