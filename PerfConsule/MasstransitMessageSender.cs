namespace PerformanceConsole
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using GreenPipes;

    using MassTransit;
    using MassTransit.Util;

    public class MasstransitMessageSender
    {
        private readonly string user;
        private readonly string password;
        private readonly string rabbitMqHost;
        private Uri targetAddress;
        private Task<ISendEndpoint> targetEndpoint;

        public Task<ISendEndpoint> TargetEndpoint => this.targetEndpoint;

        public MasstransitMessageSender(string user, string password, string rabbitMqHost)
        {
            this.user = user;
            this.password = password;
            this.rabbitMqHost = rabbitMqHost;
        }

        public async Task SendCommands(int numberOfMessages)
        {
            Console.WriteLine("Start Masstransit sending");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
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
                    sbc.ReceiveEndpoint(
                        host,
                        "MasstransitService",
                        ep =>
                        {
                            ep.PurgeOnStartup = true;
                            ep.PrefetchCount = 100;
                            this.targetAddress = ep.InputAddress;
                            ep.UseConcurrencyLimit(10);
                    //        //        ep.UseMessageScope();
                            ep.Consumer(() => new MessageHandler());
                        });
                });

            TaskUtil.Await(() => busControl.StartAsync());
            this.targetAddress = new Uri(this.rabbitMqHost + "/MasstransitService");
            this.targetEndpoint = busControl.GetSendEndpoint(this.targetAddress);

            ISendEndpoint endpoint = await this.targetEndpoint;

            var tasks = new List<Task>();

            for (var i = 0; i < numberOfMessages; i++)
            {
                tasks.Add(endpoint.Send<IMyMessage>(new
                {
                    Number = i,
                    Description = "MyMessage"
                }));
            }

            await Task.WhenAll(tasks.ToArray());
            Console.WriteLine($"Finished Masstransit sending in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}
