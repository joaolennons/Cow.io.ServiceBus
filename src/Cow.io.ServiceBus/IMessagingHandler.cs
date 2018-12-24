using System.Threading.Tasks;

namespace Cow.io.ServiceBus
{
    public interface IMessagingHandler<in T> where T : IMessage
    {
        Task Handle(T message);
    }
}
