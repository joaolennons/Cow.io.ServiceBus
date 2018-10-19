using Microsoft.Azure.ServiceBus;

namespace Cow.io.AzureServiceBus
{
    internal interface IAzureServiceQueue<T>
    {
        IQueueClient Client { get; }
    }
}