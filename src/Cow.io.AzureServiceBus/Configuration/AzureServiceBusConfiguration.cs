using Cow.io.ServiceBus;
using System;
using System.Collections.Generic;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceBusConfiguration : IServiceBusConfiguration
    {
        public string ConnectionString { get; internal set; }
        public Dictionary<Type, string> Queues { get; internal set; }
        public Dictionary<Type, Tuple<string, string>> Topics { get; internal set; }

        public AzureServiceBusConfiguration()
        {
            Queues = new Dictionary<Type, string>();
            Topics = new Dictionary<Type, Tuple<string, string>>();
        }
    }
}
