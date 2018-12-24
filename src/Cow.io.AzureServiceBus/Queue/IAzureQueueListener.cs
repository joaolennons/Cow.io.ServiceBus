using Microsoft.Azure.ServiceBus;

namespace Cow.io.AzureServiceBus
{
    internal interface IAzureQueueListener<T> : IAzureQueueListener
    {
    }

    internal interface IAzureQueueListener
    {
        IQueueClient Client { get; }
    }
}