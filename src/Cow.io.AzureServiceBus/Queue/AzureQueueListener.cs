using Cow.io.ServiceBus;
using Microsoft.Azure.ServiceBus;

namespace Cow.io.AzureServiceBus
{
    internal class AzureQueueListener<T> : IAzureQueueListener<T>
    {
        private readonly IQueueClient _client;
        public AzureQueueListener(Queue<T> queue, IHasConnectionString connectionStringHandler)
        {
            _client = new QueueClient(connectionStringHandler.ConnectionString, queue.QueueName);
        }

        public IQueueClient Client => _client;
    }
}
