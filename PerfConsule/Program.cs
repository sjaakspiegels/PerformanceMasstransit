
namespace PerformanceConsole
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("test");

            var rabbitMqSender = new RabbitMqMessageSender("TestMessages", "TestMessages", "rabbitmq://rabbitmq.voogdlocal.com");
            var masstransitSender = new MasstransitMessageSender("TestMessages", "TestMessages", "rabbitmq://rabbitmq.voogdlocal.com");


            var lst = new List<Task>
            {
                rabbitMqSender.SendCommands(10000),
                masstransitSender.SendCommands(10000)
            };
            Task.WaitAll(lst.ToArray());
        }
    }
}
