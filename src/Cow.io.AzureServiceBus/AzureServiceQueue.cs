using Cow.io.ServiceBus.Queue;
using Microsoft.Azure.ServiceBus;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceQueue<T> : IAzureServiceQueue<T>
    {
        private readonly IQueueClient _client;
        public AzureServiceQueue(Queue<T> queue, IHasConnectionString connectionStringHandler)
        {
            _client = new QueueClient(connectionStringHandler.ConnectionString, queue.QueueName);
        }

        public IQueueClient Client => _client;
    }
}
