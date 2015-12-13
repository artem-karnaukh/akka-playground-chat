using Akka.Actor;
using Akka.Configuration;
using AkkaPlayground.Core.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Akka.Routing;
using System.Threading;
using Akka.Remote.Routing;
using AkkaPlayground.Messages.Commands;

namespace AkkaPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatApp app = new ChatApp();
            app.Start();
        }
    }
}
