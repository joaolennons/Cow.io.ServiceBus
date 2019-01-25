using System.Threading.Tasks;
using Cow.io.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceBusTopicPublisher<T> : IPublisher<T> where T : IMessage
    {
        private readonly ILogger<IMessage> _logger;
        private readonly IAzureTopicWriter<T> _azureTopic;
        public AzureServiceBusTopicPublisher(IAzureTopicWriter<T> azureTopic, ILogger<IMessage> logger)
        {
            _azureTopic = azureTopic;
            _logger = logger;
        }

        public async Task Handle(T message)
        {
            var @event = new Message(message);
            await _azureTopic.Client.SendAsync(@event);
            await _azureTopic.Client.CloseAsync();
            _logger.LogInformation($@"[{@event.MessageId}] Message With body: {JsonConvert.SerializeObject(message)}
            was published at {_azureTopic.Client.TopicName} topic");
        }
    }
}