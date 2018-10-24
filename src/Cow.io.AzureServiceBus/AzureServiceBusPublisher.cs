using Cow.io.ServiceBus.Queue;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceBusPublisher<T> : IPublisher<T> where T : IMessage
    {
        private readonly IAzureQueueListener<T> _azureQueue;
        public AzureServiceBusPublisher(IAzureQueueListener<T> azureQueue)
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
