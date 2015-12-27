using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazelcast.Client;
using Hazelcast.Config;
using Hazelcast.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.Hazelcast
{
    public static class DependencyResolverExtensions
    {

        public static void UseHazelcast(this IDependencyResolver dependencyResolver, HzConfiguration configuration, IHazelcastInstance instance)
        {
            var bus = new HzScaleoutMessageBus(dependencyResolver, configuration, instance);
            dependencyResolver.Register(typeof(IMessageBus), () => bus);
        }

        public static void UseHazelcast(this IDependencyResolver dependencyResolver, HzConfiguration configuration, string[] hzUrls)
        {
            Environment.SetEnvironmentVariable("hazelcast.logging.level", "All");
            Environment.SetEnvironmentVariable("hazelcast.logging.type", "console");
            var config = new ClientConfig();
            config.GetNetworkConfig().AddAddress(hzUrls);
            config.GetNetworkConfig().SetConnectionAttemptLimit(20);

            config.GetSerializationConfig()
                .AddPortableFactory(HzConfiguration.FactoryId, new HzPortableFactory());

            var client = HazelcastClient.NewHazelcastClient(config);
            UseHazelcast(dependencyResolver, configuration, client);
        }
    }
}
