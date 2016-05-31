using Akka.Actor;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Topshelf;

namespace AkkaPlayground.Lighthouse
{
    class Program
    {
        static int Main(string[] args)
        {

            return (int)HostFactory.Run(x =>
            {
                x.SetServiceName("Lighthouse");
                x.SetDisplayName("Lighthouse Service Discovery");
                x.SetDescription("Lighthouse Service Discovery for Akka.NET Clusters");

                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.Service<LighthouseService>();
                x.EnableServiceRecovery(r => r.RestartService(1));
            });
        }
    }

    public class LighthouseService : ServiceControl
    {
        private readonly string _ipAddress;
        private readonly int? _port;

        private ActorSystem _lighthouseSystem;

        public LighthouseService() : this(null, null) { }

        public LighthouseService(string ipAddress, int? port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public bool Start(HostControl hostControl)
        {
            _lighthouseSystem = LighthouseHostFactory.LaunchLighthouse(_ipAddress, _port);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            var termination = _lighthouseSystem.Terminate();
            Task.WaitAll(termination);
            return true;
        }
    }

    public static class LighthouseHostFactory
    {
        public static ActorSystem LaunchLighthouse(string ipAddress = null, int? specifiedPort = null)
        {
            var systemName = "lighthouse";
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            var clusterConfig = section.AkkaConfig;

            var lighthouseConfig = clusterConfig.GetConfig("lighthouse");
            if (lighthouseConfig != null)
            {
                systemName = lighthouseConfig.GetString("actorsystem", systemName);
            }

            var remoteConfig = clusterConfig.GetConfig("akka.remote");
            ipAddress = ipAddress ??
                        remoteConfig.GetString("helios.tcp.public-hostname") ??
                        "127.0.0.1"; //localhost as a final default
            int port = specifiedPort ?? remoteConfig.GetInt("helios.tcp.port");

            if (port == 0) throw new Akka.Configuration.ConfigurationException("Need to specify an explicit port for Lighthouse. Found an undefined port or a port value of 0 in App.config.");

            var selfAddress = string.Format("akka.tcp://{0}@{1}:{2}", systemName, ipAddress, port);
            var seeds = clusterConfig.GetStringList("akka.cluster.seed-nodes");
            if (!seeds.Contains(selfAddress))
            {
                seeds.Add(selfAddress);
            }

            var injectedClusterConfigString = seeds.Aggregate("akka.cluster.seed-nodes = [", (current, seed) => current + (@"""" + seed + @""", "));
            injectedClusterConfigString += "]";

            var finalConfig = ConfigurationFactory.ParseString(
                string.Format(@"akka.remote.helios.tcp.public-hostname = {0} 
akka.remote.helios.tcp.port = {1}", ipAddress, port))
                .WithFallback(ConfigurationFactory.ParseString(injectedClusterConfigString))
                .WithFallback(clusterConfig);

            return ActorSystem.Create(systemName, finalConfig);
        }
    }
}
