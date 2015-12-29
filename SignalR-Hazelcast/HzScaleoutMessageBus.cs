using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazelcast.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.Hazelcast
{
    class HzScaleoutMessageBus : ScaleoutMessageBus
    {
        private readonly ITopic<HzMessage> _topic;
        private readonly IAtomicLong _counter;
        private readonly ILock _hzLock;

        public HzScaleoutMessageBus(IDependencyResolver resolver, HzConfiguration configuration, IHazelcastInstance hzInstance) : base(resolver, configuration)
        {
            _topic = hzInstance.GetTopic<HzMessage>(configuration.TopicName);
            _topic.AddMessageListener(msg =>
            {
                // TODO real logs
                //Console.Out.WriteLine("Just received something");
                var hzMessage = msg.GetMessageObject();
                lock (_hzLock)
                {
                    OnReceived(0, hzMessage.Id, hzMessage.ScaleoutMessage);
                }
            });

            _counter = hzInstance.GetAtomicLong(configuration.CounterName);
            _hzLock = hzInstance.GetLock(configuration.LockName);
        }


        protected override Task Send(int streamIndex, IList<Message> messages)
        {
            // TODO real logs
            //Console.Out.WriteLine("About to send something");
            return Send(messages);
        }

        protected override Task Send(IList<Message> messages)
        {
            // TODO real logs
            //Console.Out.WriteLine("About to send something 2");
            _hzLock.Lock();
            try
            {
                var sqn = (ulong) _counter.IncrementAndGet();
                var hzMessage = new HzMessage {Id = sqn, ScaleoutMessage = new ScaleoutMessage(messages)};
                _topic.Publish(hzMessage);
            }
            finally
            {
                _hzLock.Unlock();
            }
            return Task.WhenAll();
        }
    }
}
