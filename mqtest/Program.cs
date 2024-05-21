using MassTransit;
using static mqtest.customersended;

namespace mqtest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("event-listener", ec =>
                {
                    ec.Consumer<CustomerASendedEventHandler>();
                    ec.Consumer<CustomerBSendedEventHandler>();
                });
            });

            busControl.Start(); // 启动总线

            Console.WriteLine("Listening for messages.. Press any key to exit");
            Console.ReadKey();

            busControl.Stop();

        }
    }
}
