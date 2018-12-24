using App.Controllers;
using Cow.io.ServiceBus;
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
