using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue.Messaging.Extensions;
using MessageQueue.Messaging.Spec;
using Microsoft.ServiceBus.Messaging;

namespace MessageQueue.Messaging.Impl.Azure
{
    public class ServiceBusMessageQueue : MessageQueueBase
    {

        private QueueClient _queueClient;
        private const string _connectionString = "";

        public override void InitialiseOutbound(string address, MessagePattern pattern, Dictionary<string, object> properties = null)
        {
            Initialize(Direction.Outbound, address, pattern, properties);
            var factory = MessagingFactory.CreateFromConnectionString(_connectionString);
            _queueClient = factory.CreateQueueClient(Address);
        }

        public override void InitialiseInbound(string address, MessagePattern pattern, Dictionary<string, object> properties = null)
        {
            Initialize(Direction.Inbound, address, pattern, properties);
            var factory = MessagingFactory.CreateFromConnectionString(_connectionString);
            _queueClient = factory.CreateQueueClient(Address);
        }

        public override void Send(Message message)
        {
            var brokeredMessage = new BrokeredMessage(message.ToJsonStream(), true);
            _queueClient.Send(brokeredMessage);
        }

        public override void Receive(Action<Message> onMessageReceived)
        {
            throw new NotImplementedException();
        }

        protected override string GetAddress(string name)
        {
            switch (name.ToLower())
            {
                case "doesuserexist":
                    return "doesuserexist";

                default:
                    return name;
            }
        }

        public override IMessageQueue GetResponseQueue()
        {
             
        }

        public override IMessageQueue GetReplyQueue(Message message)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Listen(Action<Message> onMessageReceived)
        {
            throw new NotImplementedException();
        }
    }
}
