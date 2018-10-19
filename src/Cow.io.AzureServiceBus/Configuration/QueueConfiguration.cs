using Cow.io.ServiceBus.Queue;
using System;
using System.Collections.Generic;

namespace Cow.io.AzureServiceBus
{
    internal class QueueConfiguration : IQueueConfiguration
    {
        public string ConnectionString { get; internal set; }
        public Dictionary<Type, string> Queues { get; internal set; }

        public QueueConfiguration()
        {
            Queues = new Dictionary<Type, string>();
        }
    }
}
