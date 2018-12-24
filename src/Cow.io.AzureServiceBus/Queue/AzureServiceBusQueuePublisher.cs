using Cow.io.ServiceBus;
using System.Threading.Tasks;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceBusQueuePublisher<T> : IPublisher<T> where T : IMessage
    {
        private readonly IAzureQueueListener<T> _azureQueue;
        public AzureServiceBusQueuePublisher(IAzureQueueListener<T> azureQueue)
        {
            _azureQueue = azureQueue;
        }

        public async Task Handle(T message)
        {
            await _azureQueue.Client.SendAsync(new Message(message));
            await _azureQueue.Client.CloseAsync();
        }
    }
}
