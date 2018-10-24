using App.Controllers;
using Cow.io.ServiceBus.Queue;
using System.Threading.Tasks;

namespace App.Subscribers
{
    public class SubscriberDefault : ISubscribe<MessageSaga>
    {
        public async Task Handle(MessageSaga message)
        {
            await Task.FromResult("Hello from subscriber!");
        }
    }
}
