using Cow.io.ServiceBus.Queue;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IPublisher<Message> _publisher;

        public ValuesController(IPublisher<Message> publisher)
        {
            _publisher = publisher;
        }

        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("Hello!");
        }

        [HttpPost]
        public async Task Publish([FromBody] Message message)
        {
            await _publisher.Handle(message);
        }
    }

    public class Message : IMessage
    {
        public string Text { get; set; }
    }
}
