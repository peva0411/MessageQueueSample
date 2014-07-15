using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.Messaging.Spec
{
    public interface IMessageQueue : IDisposable
    {
        string Address { get; }

        Dictionary<string, object> Properties { get; }

        void InitialiseOutbound(string address, MessagePattern pattern, Dictionary<string,object> properties = null);

        void InitialiseInbound(string address, MessagePattern pattern, Dictionary<string,object> properties = null);

        void Send(Message message);

        void Receive(Action<Message> onMessageReceived);

        void Listen(Action<Message> onMessageReceived);

        IMessageQueue GetResponseQueue();

        IMessageQueue GetReplyQueue(Message message);
    }
}
