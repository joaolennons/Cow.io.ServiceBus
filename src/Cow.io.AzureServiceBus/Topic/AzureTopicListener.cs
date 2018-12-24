using Cow.io.ServiceBus;
using Microsoft.Azure.ServiceBus;

namespace Cow.io.AzureServiceBus
{
    internal class AzureTopicListener<T> : IAzureTopicListener<T>
    {
        private readonly ISubscriptionClient _client;
        public AzureTopicListener(Topic<T> topic, IHasConnectionString connectionStringHandler)
        {
            _client = new SubscriptionClient(connectionStringHandler.ConnectionString, topic.TopicName, topic.SubscriptionName);
        }

        public ISubscriptionClient Client => _client;
    }
}