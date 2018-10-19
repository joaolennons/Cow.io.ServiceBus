namespace Cow.io.ServiceBus.Queue
{
    public interface IPublisher<T> : IMessagingHandler<T> where T : IMessage
    {
    }
}
