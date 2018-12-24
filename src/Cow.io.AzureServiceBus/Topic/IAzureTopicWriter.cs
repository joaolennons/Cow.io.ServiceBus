using Microsoft.Azure.ServiceBus;

namespace Cow.io.AzureServiceBus
{
    internal interface IAzureTopicWriter<T> : IAzureTopicWriter
    {
    }

    internal interface IAzureTopicWriter
    {
        ITopicClient Client { get; }
    }
}