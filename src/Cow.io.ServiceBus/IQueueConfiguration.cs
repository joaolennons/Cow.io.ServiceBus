using System;
using System.Collections.Generic;

namespace Cow.io.ServiceBus
{
    public interface IQueueConfiguration : IHasConnectionString
    {
        Dictionary<Type, string> Queues { get; }
    }
}
