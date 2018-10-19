using Cow.io.ServiceBus.Queue;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceBusPublisher<T> : IPublisher<T> where T : IMessage
    {
        private readonly IAzureServiceQueue<T> _azureQueue;
        public AzureServiceBusPublisher(IAzureServiceQueue<T> azureQueue)
        {
            _azureQueue = azureQueue;
        }

        public async Task Handle(T message)
        {
            var enqueueMessage = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
            await _azureQueue.Client.SendAsync(enqueueMessage);
            await _azureQueue.Client.CloseAsync();
        }
    }
}
