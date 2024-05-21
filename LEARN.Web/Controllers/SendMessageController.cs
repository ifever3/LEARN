using MassTransit;
using message;
using Microsoft.AspNetCore.Mvc;

namespace LEARN.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendMessageController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public SendMessageController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageAsync()
        {
            try
            {
                await PublishMessagesAsync(0,10);
                return Ok("消息发布成功");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"发生错误: {ex.Message}");
            }
        }

        public async Task PublishMessagesAsync(int start, int end)
        {
            var tasks = new List<Task>();

            for (int i = start; i < end; i++)
            {
                int index = i;
                var task = Task.Run(async () =>
                {
                    await _publishEndpoint.Publish<sendedevent>(new sendedevent("tom"+index, "math"));
                    Console.WriteLine("消息发布成功 " + index);
                });            
                tasks.Add(task);          
            }
            await Task.WhenAll(tasks);
        }
    }
}
