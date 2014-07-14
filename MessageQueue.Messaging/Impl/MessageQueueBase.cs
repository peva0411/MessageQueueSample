using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue.Messaging.Spec;

namespace MessageQueue.Messaging.Impl
{
    public abstract class MessageQueueBase:IDisposable
    {
        public Direction Direction { get; set; }
        public MessagePattern Pattern { get; set; }
        public string Address { get; private set; }
        public Dictionary<string, object> Properties { get; set; }

        protected virtual void Initialize(Direction direction, string name, MessagePattern pattern, Dictionary<string, object> properties)
        {
            Direction = direction;
            Pattern = pattern;
            Address = GetAddress(name);
            Properties = properties ?? new Dictionary<string, object>();
        }

        protected void RequireProperty<T>(string name)
        {
            
        }

        protected abstract void InitializeOutbound(string name, MessagePattern pattern, Dictionary<string, object> properties);

        protected abstract void IntializeInbound(string name, MessagePattern pattern, Dictionary<string, object> properties);

        protected abstract void Send(Message message);

        protected abstract void Listen(Action<Message> onMessageReceived);

        protected abstract void Receive(Action<Message> onMessageReceived);

        protected abstract string GetAddress(string name);

        protected abstract IMessageQueue GetResponseQueue();

        protected abstract IMessageQueue GetReplyQueue(Message message);

        public abstract void Dispose();
    }
}
