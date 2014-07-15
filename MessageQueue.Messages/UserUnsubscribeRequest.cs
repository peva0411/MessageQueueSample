using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.Messages
{
    public class UserUnsubscribeRequest
    {
        public string EmailAddress { get; set; }
    }
}
