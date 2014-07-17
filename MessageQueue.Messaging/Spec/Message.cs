using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue.Messaging.Extensions;
using Newtonsoft.Json;

namespace MessageQueue.Messaging.Spec
{
    public class Message
    {
        public Type BodyType
        {
            get { return Body.GetType(); }
        }

        private object _body;

        public object Body
        {
            get { return _body; }
            set
            {
                _body = value;
                MessageType = _body.GetMessageType();
            }
        }

        public string ResponseAddress { get; set; }

        public string MessageType { get; set; }

        public T BodyAs<T>()
        {
            return (T)Body;
        }

        public static Message FromJson(Stream jsonStream)
        {
            var message = jsonStream.ReadFromJson<Message>();

            var bodyStream = message.Body.ToJsonStream();

            message.Body = bodyStream.ReadFromJson(message.MessageType);

            return message;
        }

        public static Message FromJson(string json)
        {
            var message = JsonConvert.DeserializeObject<Message>(json);

            var bodyStream = message.Body.ToJsonStream();

            message.Body = bodyStream.ReadFromJson(message.MessageType);

            return message;
        }

    }
}
