using Cow.io.ServiceBus;
using Microsoft.Azure.ServiceBus;

namespace Cow.io.AzureServiceBus
{
    internal class AzureTopicWriter<T> : IAzureTopicWriter<T>
    {
        private readonly ITopicClient _client;
        public AzureTopicWriter(Topic<T> topic, IHasConnectionString connectionStringHandler)
        {
            _client = new TopicClient(connectionStringHandler.ConnectionString, topic.TopicName);
        }

        public ITopicClient Client => _client;
    }
}
