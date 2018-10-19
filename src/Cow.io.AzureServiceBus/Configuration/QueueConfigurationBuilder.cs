using Cow.io.ServiceBus.Queue;

namespace Cow.io.AzureServiceBus
{
    internal class QueueConfigurationBuilder : IQueueConfigurationBuilder
    {
        private readonly QueueConfiguration _configuration;
        public QueueConfigurationBuilder()
        {
            _configuration = new QueueConfiguration();
        }
        public IQueueConfigurationBuilder WithConnectionString(string connectionString)
        {
            _configuration.ConnectionString = connectionString;
            return this;
        }

        public IQueueConfigurationBuilder WithQueue<T>(string queueName)
        {
            _configuration.Queues.Add(typeof(T), queueName);
            return this;
        }

        public IQueueConfiguration Build()
        {
            return _configuration;
        }
    }
}
