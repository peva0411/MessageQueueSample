using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue.Messaging.Spec;

namespace MessageQueue.Messaging.Impl
{
    public abstract class MessageQueueBase:IMessageQueue,IDisposable
    {
        public Direction Direction { get; set; }
        public MessagePattern Pattern { get; set; }
        public string Address { get; private set; }
        public Dictionary<string, object> Properties { get; set; }


        public abstract void InitialiseOutbound(string address, MessagePattern pattern,
            Dictionary<string, object> properties = null);

        public abstract void InitialiseInbound(string address, MessagePattern pattern,
            Dictionary<string, object> properties = null);

        public abstract void Send(Message message);

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

        public abstract void Listen(Action<Message> onMessageReceived);

        public abstract void Receive(Action<Message> onMessageReceived);

        protected abstract string GetAddress(string name);

        public abstract IMessageQueue GetResponseQueue();

        public abstract IMessageQueue GetReplyQueue(Message message);

        public abstract void Dispose();
    }
}
