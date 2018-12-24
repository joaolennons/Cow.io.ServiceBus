namespace Cow.io.ServiceBus
{
    public interface IPublisher<T> : IMessagingHandler<T> where T : IMessage
    {
    }
}
