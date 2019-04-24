namespace PerformanceConsole
{
    using System;
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

        public Task SendCommands(int numberOfMessages)
        {
            return Task.Run(() =>
            {
                using (var connection = this.factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        var messageProperties = channel.CreateBasicProperties();
            //            messageProperties.Persistent = true;

                        channel.ConfirmSelect();

              //          var tasks = new List<Task>();

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
                //            tasks.Add(Task.Run(() =>
                  //          {
                                channel.BasicPublish("RabbitMqService", "", messageProperties, body);
                                channel.WaitForConfirms();
                    //        }));
                        }

                      //  Task.WaitAll(tasks.ToArray());
                    }
                }
            });
        }

    }
}
