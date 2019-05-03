
namespace PerformanceConsole
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("start test");

            var rabbitMqSender = new RabbitMqMessageSender("TestMessages", "TestMessages", "rabbitmq://rabbitmq.voogdlocal.com");
            var masstransitSender = new MasstransitMessageSender("TestMessages", "TestMessages", "rabbitmq://rabbitmq.voogdlocal.com");

 //           var rabbitMqSender = new RabbitMqMessageSender("guest", "guest", "rabbitmq://localhost");
//            var masstransitSender = new MasstransitMessageSender("guest", "guest", "rabbitmq://localhost");

 //           rabbitMqSender.SendCommands(100000);
            await masstransitSender.SendCommands(100000);
        }
    }
}
