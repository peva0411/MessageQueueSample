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
            if (args[0] == "userentered")
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

            if (args[0] == "mass")
            {
                var upper = Int32.Parse(args[1]);

                for (int i = 0; i < upper; i++)
                {
                    var email = string.Format("address{0}@email.com", i);

                    var doesUserExistRequest = new UserUnsubscribeRequest
                    {
                        EmailAddress = email
                    };

                    var queue = MessageQueueFactory.CreateOutbound("unsubscribe", MessagePattern.FireAndForget);

                    queue.Send(new Message()
                    {
                        Body = doesUserExistRequest
                    });

                    Console.WriteLine("Sent request for: {0}", email);
                }
            }

        }
    }
}
