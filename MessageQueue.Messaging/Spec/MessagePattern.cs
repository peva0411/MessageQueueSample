﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.Messaging.Spec
{
    public enum MessagePattern
    {
        FireAndForget,
        RequestResponse,
        PublishSubscribe
    }
}
