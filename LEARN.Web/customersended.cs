using MassTransit;
using message;

namespace LEARN.Web
{
    public class customersended
    {
        public class CustomerASendedEventHandler : IConsumer<sendedevent>
        {
            public Task Consume(ConsumeContext<sendedevent> context)
            {
                Console.WriteLine($"A收到{context.Message.name}通知！");
                return Task.CompletedTask;
            }
        }
        /// <summary>
        /// 具体事件处理类顾客B
        /// </summary>
        public class CustomerBSendedEventHandler : IConsumer<sendedevent>
        {
            public Task Consume(ConsumeContext<sendedevent> context)
            {
                Console.WriteLine($"B收到{context.Message.major}通知！");
                return Task.CompletedTask;
            }
        }
    }
}
