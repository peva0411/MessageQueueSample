using System;
using MessageQueue.Messages;
using MessageQueue.Messaging;
using MessageQueue.Messaging.Spec;

namespace MessageQueue.Handler
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageQueue = MessageQueueFactory.CreateInbound("unsubscribe", MessagePattern.FireAndForget);

            messageQueue.Listen(m =>
            {
                var request = m.BodyAs<UserUnsubscribeRequest>();
                Console.WriteLine("Request received: {0} \n\t User: {1}",DateTime.Now, request.EmailAddress);
            });

        }

 
    }
}
