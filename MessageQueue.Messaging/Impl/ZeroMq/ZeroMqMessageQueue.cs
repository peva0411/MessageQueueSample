using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue.Messaging.Extensions;
using MessageQueue.Messaging.Spec;
using ZMQ;

namespace MessageQueue.Messaging.Impl.ZeroMq
{
    public class ZeroMqMessageQueue: MessageQueueBase
    {
        private Socket _socket;
        private static Context _Context;
        private static object _ContextLock = new object();

        protected override void InitialiseOutbound(string name, MessagePattern pattern, Dictionary<string, object> properties)
        {
            Initialize(Direction.Outbound, name, pattern, properties);
            EnsureContext();
            switch (Pattern)
            {
                    case MessagePattern.RequestResponse:
                        _socket = _Context.Socket(SocketType.REQ);
                        _socket.Connect(Address);
                        break;
            }
        }

        private void EnsureContext()
        {
            if (_Context == null)
            {
                lock (_ContextLock)
                {
                    if (_Context == null)
                    {
                        _Context = new Context();
                    }
                }
            }
        }

        protected override void InitialiseInbound(string name, MessagePattern pattern, Dictionary<string, object> properties)
        {
            Initialize(Direction.Inbound, name, pattern, properties);
            EnsureContext();
            switch (Pattern)
            {
                case MessagePattern.RequestResponse:
                    _socket = _Context.Socket(SocketType.REP);
                    _socket.Bind(Address);
                    break;
            }
        }

        protected override void Send(Message message)
        {
            var json = message.ToJsonString();
            _socket.Send(json, Encoding.UTF8);
        }

        protected override void Listen(Action<Message> onMessageReceived)
        {
            while (true)
            {
                Receive(onMessageReceived);
            }
        }

        protected override void Receive(Action<Message> onMessageReceived)
        {
            var inbound = _socket.Recv(Encoding.UTF8);
            var message = Message.FromJson(inbound);
            onMessageReceived(message);
        }

        protected override string GetAddress(string name)
        {
            switch (name.ToLower())
            {
                case"doesuserexist":
                    return "tcp://127.0.0.1:5556";
                    break;
                default:
                    throw new ArgumentException(string.Format("Uknown queue name: {0}", name));
            }
        }

        public override IMessageQueue GetResponseQueue()
        {
            return this;
        }

        public override IMessageQueue GetReplyQueue(Message message)
        {
            return this;
        }

        public override void Dispose(bool disposing)
        {
            if (disposing && _socket != null)
            {
                _socket.Dispose();
            }
        }
    }
}
