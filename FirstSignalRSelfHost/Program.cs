using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using SignalR.Hazelcast;

namespace FirstSignalRSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8081";

            using (WebApp.Start(url, app =>
            {
                var hzConfiguration = new HzConfiguration();
          
                app.UseCors(CorsOptions.AllowAll);
          
                var resolver = new DefaultDependencyResolver();
                resolver.UseHazelcast(hzConfiguration, new[] { "127.0.0.1:5701", "127.0.0.1:5702", "127.0.0.1:5703" });
                var assemblyLocator = new AssemblyLocator();
                resolver.Register(typeof(IAssemblyLocator), () => assemblyLocator);
                var hubConfiguration = new HubConfiguration
                {
                    Resolver = resolver,
                };
                app.MapSignalR(hubConfiguration);
            }))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            };

        }
    }

    class AssemblyLocator : IAssemblyLocator
    {
        public IList<Assembly> GetAssemblies()
        {
            return new[] { typeof(MyHub).Assembly };
        }
    }


    public class MyHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}
