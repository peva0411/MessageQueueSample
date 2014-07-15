using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue.Messaging;
using MessageQueue.Messaging.Spec;

namespace MessageQueue.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var exists = false;

            var doesUserExistRequest = new DoesUserExistRequest
            {
                EmailAddress = "peva0411@github.com"
            };

            var queue = MessageQueueFactory.CreateOutbound("doesuserexist", MessagePattern.RequestResponse);
            var responseQueue = queue.GetResponseQueue();

            queue.Send(new Message()
            {
                Body = doesUserExistRequest,
                ResponseAddress = responseQueue.Address
            });

            responseQueue.Receive(x => exists = x.BodyAs<DoesUserExistResponse>().Exists);

            Console.WriteLine("User: {0}", exists);
        }
    }

    public class DoesUserExistResponse
    {
        public bool Exists { get; set; }
    }

    public class DoesUserExistRequest
    {
        public string EmailAddress { get; set; }
    }
}
