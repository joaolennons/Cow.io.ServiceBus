
using Cow.io.ServiceBus;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceBusConfigurationBuilder : IServiceBusConfigurationBuilder
    {
        private readonly AzureServiceBusConfiguration _configuration;
        public AzureServiceBusConfigurationBuilder()
        {
            _configuration = new AzureServiceBusConfiguration();
        }

        public IServiceBusConfigurationBuilder WithConnectionString(string connectionString)
        {
            _configuration.ConnectionString = connectionString;
            return this;
        }

        public IServiceBusConfigurationBuilder WithQueue<T>(string queueName)
        {
            _configuration.Queues.Add(typeof(T), queueName);
            return this;
        }

        public IServiceBusConfigurationBuilder WithTopic<T>(string topicName)
        {
            _configuration.Topics.Add(typeof(T), new System.Tuple<string, string>(topicName, null));
            return this; 
        }

        public IServiceBusConfigurationBuilder WithTopic<T>(string topicName, string subscriptionName)
        {
            _configuration.Topics.Add(typeof(T), new System.Tuple<string, string>(topicName, subscriptionName));
            return this;
        }

        internal IServiceBusConfiguration Build()
        {
            return _configuration;
        }
    }
}
