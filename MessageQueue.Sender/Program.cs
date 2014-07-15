using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue.Messages;
using MessageQueue.Messaging;
using MessageQueue.Messaging.Spec;

namespace MessageQueue.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter Email:");
                var email = Console.ReadLine();

                var doesUserExistRequest = new UserUnsubscribeRequest
                {
                    EmailAddress = email
                };

                var queue = MessageQueueFactory.CreateOutbound("unsubscribe", MessagePattern.FireAndForget);

                queue.Send(new Message()
                {
                    Body = doesUserExistRequest
                });
            }
        }
    }
}
