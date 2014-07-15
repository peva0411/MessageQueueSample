using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue.Messaging.Impl.Msmq;
using MessageQueue.Messaging.Impl.ZeroMq;
using MessageQueue.Messaging.Spec;

namespace MessageQueue.Messaging
{
    public static class MessageQueueFactory
    {
        private static Dictionary<string, IMessageQueue> _Queues = new Dictionary<string, IMessageQueue>();

        public static IMessageQueue CreateInbound(string name, MessagePattern pattern,
            Dictionary<string, object> properties = null)
        {
            var key = string.Format("{0}:{1}:{2}", Direction.Inbound, name, pattern);
            if (_Queues.ContainsKey(key))
                return _Queues[key];

            var queue = Create();
            queue.InitialiseInbound(name, pattern, properties);
            _Queues[key] = queue;
            return _Queues[key];
        }

        public static IMessageQueue CreateOutbound(string name, MessagePattern pattern,
            Dictionary<string, object> properties = null)
        {
            var key = string.Format("{0}:{1}:{2}", Direction.Inbound, name, pattern);
            if (_Queues.ContainsKey(key))
                return _Queues[key];

            var queue = Create();
            queue.InitialiseOutbound(name, pattern, properties);
            _Queues[key] = queue;
            return _Queues[key];
        }

        private static IMessageQueue Create()
        {
            //return new MsmqMessageQueue();
            return new ZeroMqMessageQueue();
        }
    }
}
