using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MessageQueue.Messaging.Extensions;
using MessageQueue.Messaging.Spec;
using msmq = System.Messaging;

namespace MessageQueue.Messaging.Impl.Msmq
{
    public class MsmqMessageQueue : MessageQueueBase
    {
        private msmq.MessageQueue _queue;
        private bool _useTemporaryQueue;


        public override void InitialiseOutbound(string name, MessagePattern pattern, Dictionary<string, object> properties = null)
        {
            throw new NotImplementedException();
        }

        public override void InitialiseInbound(string name, MessagePattern pattern, Dictionary<string, object> properties = null)
        {
            Initialize(Direction.Inbound, name, pattern, properties);
            switch (pattern)
            {
                case MessagePattern.PublishSubscribe:
                    _queue = new msmq.MessageQueue(Address);
                    break;

                case MessagePattern.RequestResponse:
                    _queue = _useTemporaryQueue ? msmq.MessageQueue.Create(Address) : new msmq.MessageQueue(Address);
                    break;
                default:
                    _queue = new msmq.MessageQueue(Address);
                    break;
            }
        }

        public override void Send(Message message)
        {
            var outbound = new msmq.Message();
            outbound.BodyStream = message.ToJsonStream();
            if (!string.IsNullOrEmpty(message.ResponseAddress))
            {
                outbound.ResponseQueue = new msmq.MessageQueue(message.ResponseAddress);
            }
            _queue.Send(outbound);
        }

        public override void Listen(Action<Message> onMessageReceived)
        {
            while (true)
            {
                Receive(onMessageReceived);
            }
        }

        public override void Receive(Action<Message> onMessageReceived)
        {
            var inbound = _queue.Receive();
            var message = Message.FromJson(inbound.BodyStream);
            onMessageReceived(message);
        }

        protected override string GetAddress(string name)
        {
            if (Pattern == MessagePattern.RequestResponse && Direction == Direction.Inbound)
            {
                _useTemporaryQueue = true;
                return string.Format(".\\private$\\{0}", Guid.NewGuid().ToString().Substring(0, 6));
            }

            switch (name.ToLower())
            {
                case "unsubscribe":
                    return ".\\private$\\messagequeue.unsubscribe";
                case "doesuserexist":
                    return ".\\private$\\messagequeue.doesuserexist";
                case "unsubscribe-legacy":
                    return ".\\private$\\messagequeue.unsubscribe-legacy";
                case "unsubscribed-event":
                    return "FormatName:MULTICAST=234.1.1.2:8001";
                case "unsubscribe-crm":
                    return ".\\private$\\messagequeue.unsubscribe-crm";
                case "unsubscribe-fulfilment":
                    return ".\\private$\\messagequeue.unsubscribe-fulfilment";
            }

        }

        public override IMessageQueue GetResponseQueue()
        {
            throw new NotImplementedException();
        }

        public override IMessageQueue GetReplyQueue(Message message)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
