using Cow.io.ServiceBus.Queue;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IPublisher<MessageSaga> _publisher;

        public ValuesController(IPublisher<MessageSaga> publisher)
        {
            _publisher = publisher;
        }

        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("Hello!");
        }

        [HttpPost]
        public async Task Publish([FromBody] MessageSaga message)
        {
            await _publisher.Handle(message);
        }
    }

    public class MessageSaga : Cow.io.ServiceBus.Queue.MessageBody
    {
        public string Text { get; set; }
    }
}
