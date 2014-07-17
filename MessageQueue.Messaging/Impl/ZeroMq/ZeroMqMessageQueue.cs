using System;
using System.Collections.Generic;
using System.IO;
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

        public override void InitialiseOutbound(string name, MessagePattern pattern, Dictionary<string, object> properties)
        {
            Initialize(Direction.Outbound, name, pattern, properties);
            EnsureContext();
            switch (Pattern)
            {
                    case MessagePattern.RequestResponse:
                        _socket = _Context.Socket(SocketType.REQ);
                        _socket.Connect(Address);
                        break;
                case MessagePattern.FireAndForget:
                    _socket = _Context.Socket(SocketType.PUSH);
                    _socket.Connect(Address);
                    break;
                case MessagePattern.PublishSubscribe:
                    _socket = _Context.Socket(SocketType.PUB);
                    _socket.Bind(Address);
                    break;
            }
        }

        public override void InitialiseInbound(string name, MessagePattern pattern, Dictionary<string, object> properties)
        {
            Initialize(Direction.Inbound, name, pattern, properties);
            EnsureContext();
            switch (Pattern)
            {
                case MessagePattern.RequestResponse:
                    _socket = _Context.Socket(SocketType.REP);
                    _socket.Bind(Address);
                    break;
                case MessagePattern.FireAndForget:
                    _socket = _Context.Socket(SocketType.PULL);
                    _socket.Bind(Address);
                    break;
                case MessagePattern.PublishSubscribe:
                    _socket = _Context.Socket(SocketType.SUB);
                    _socket.Connect(Address);
                    _socket.Subscribe("", Encoding.UTF8);
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

        public override void Send(Message message)
        {
            var json = message.ToJsonString();
            _socket.Send(json, Encoding.UTF8);
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
                case "unsubscribe":
                    return "tcp://127.0.0.1:5555";
                case "unsubscribe-event":
                    return "pgm://127.0.0.1:5557;239.192.1.1:5557";
                case "unsubscribe-legacy":
                    return "pgm://127.0.0.1:5557;239.192.1.1:5557";
                case "unsubscribe-crm":
                    return "pgm://127.0.0.1:5557;239.192.1.1:5557";
                case "unsubscribe-fulfilment":
                    return "pgm://127.0.0.1:5557;239.192.1.1:5557";
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

        public override void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (disposing && _socket != null)
            {
                _socket.Dispose();
            }
        }
    }
}
