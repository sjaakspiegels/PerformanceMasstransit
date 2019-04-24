namespace PerformanceConsole
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MassTransit;

    public class MasstransitMessageSender
    {
        private readonly string user;
        private readonly string password;
        private readonly string rabbitMqHost;

        public MasstransitMessageSender(string user, string password, string rabbitMqHost)
        {
            this.user = user;
            this.password = password;
            this.rabbitMqHost = rabbitMqHost;
        }

        public Task SendCommands(int numberOfMessages)
        {
            return Task.Run(() =>
            {
                var busControl = global::MassTransit.Bus.Factory.CreateUsingRabbitMq(
                    sbc =>
                    {
                        // Host control
                        var host =
                            sbc.Host(
                                new Uri(this.rabbitMqHost),
                                h =>
                                {
                                    h.Username(this.user);
                                    h.Password(this.password);
                                    h.Heartbeat(60);
                                    h.PublisherConfirmation = false;
                                });

                        // Receive endpoint
//                        sbc.ReceiveEndpoint(
//                            host,
//                            "MasstransitService",
//                            ep =>
//                            {
//                                ep.PrefetchCount = 32;


////                                ep.UseMessageScope();
////                                ep.Consumer<MessageHandler>();
//                            });
                    });

                busControl.StartAsync().Wait();
                var task = busControl.GetSendEndpoint(new Uri(this.rabbitMqHost + "/MasstransitService"));
                var tasks = new List<Task>();

                task.Wait();
                var endpoint = task.Result;
                for (var i = 0; i < numberOfMessages; i++)
                {
                    tasks.Add(endpoint.Send<IMyMessage>(new
                    {
                        Number = i,
                        Description = "MyMessage"
                    }));
                }

                Task.WaitAll(tasks.ToArray());
            });
        }
    }
}
