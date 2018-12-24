namespace Cow.io.AzureServiceBus
{
    internal class Topic<T>
    {
        public Topic(string topicName, string subscriptionName)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }

        public string TopicName { get; }
        public string SubscriptionName { get; }
    }
}
