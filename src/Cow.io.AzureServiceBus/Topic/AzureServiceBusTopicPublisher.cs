using Cow.io.ServiceBus;
using System.Threading.Tasks;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceBusTopicPublisher<T> : IPublisher<T> where T : IMessage
    {
        private readonly IAzureTopicWriter<T> _azureTopic;
        public AzureServiceBusTopicPublisher(IAzureTopicWriter<T> azureTopic)
        {
            _azureTopic = azureTopic;
        }

        public async Task Handle(T message)
        {
            await _azureTopic.Client.SendAsync(new Message(message));
            await _azureTopic.Client.CloseAsync();
        }
    }
}