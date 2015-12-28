using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazelcast.Config;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.Hazelcast
{
    public class HzConfiguration : ScaleoutConfiguration
    {

        public static int FactoryId = 42;
        public static int ClassId = 42;

        public HzConfiguration()
        {
            FastMode = false;
            TopicName = "signalrTopic";
            CounterName = "signalrCounter";
            LockName = "signalrLock";
        }

        public bool FastMode { get; set; }

        public string TopicName { get; set; }

        public string CounterName { get; set; }
        public string LockName { get; set; }
    }
}
