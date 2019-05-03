namespace PerformanceConsole
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using RabbitMQ.Client;

    public class RabbitMqMessageSender
    {
        private readonly ConnectionFactory factory;

        public RabbitMqMessageSender(string user, string password, string rabbitMqHost)
        {
            var myUri = new Uri(rabbitMqHost);

            this.factory = new ConnectionFactory
            {
                HostName = myUri.Host,
                UserName = user,
                Password = password,
                VirtualHost = myUri.LocalPath,
                Port = (myUri.Port > 0 ? myUri.Port : -1),
                AutomaticRecoveryEnabled = true
            };
        }

        public void SendCommands(int numberOfMessages)
        {
            Console.WriteLine("Start RabbitMQ sending");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var connection = this.factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var messageProperties = channel.CreateBasicProperties();
                    channel.ExchangeDeclare("PerformanceConsole:ShowRabbitMqMessage", "fanout", true, false, null);
                    channel.ExchangeDeclare("RabbitMqService", "fanout", true, false, null);
                    channel.QueueDeclare("RabbitMqService", true, false, false, null);
                    channel.ExchangeBind("RabbitMqService", "PerformanceConsole:ShowRabbitMqMessage", "", null);
                    channel.QueueBind("RabbitMqService", "RabbitMqService", "", null);
                    for (var i = 0; i < numberOfMessages; i++)
                    {
                        var bericht = new
                        {
                            Volgnummer = 1,
                            Tekst = "Bericht"
                        };
                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bericht));
                        channel.BasicPublish("RabbitMqService", "", messageProperties, body);
                    }
                }
            }

            Console.WriteLine($"Finished RabbitMQ sending in {stopwatch.ElapsedMilliseconds} ms.");
        }

    }
}
