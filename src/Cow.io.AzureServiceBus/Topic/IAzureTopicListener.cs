using Microsoft.Azure.ServiceBus;

namespace Cow.io.AzureServiceBus
{
    internal interface IAzureTopicListener<T> : IAzureTopicListener
    {
    }

    internal interface IAzureTopicListener
    {
        ISubscriptionClient Client { get; }
    }
}