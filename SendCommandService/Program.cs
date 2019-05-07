namespace Performance.SendCommandsService
{
    using System;
    using System.ServiceProcess;
    using System.Threading;

    using log4net.Config;

    using Topshelf;
    using Topshelf.Logging;

    public class Program
    {
        /// <summary>
        /// Logging component.
        /// </summary>
        private static readonly LogWriter Log = HostLogger.Get<Program>();

        /// <summary>
        /// Begin van de AcceptatieService.
        /// </summary>
        public static void Main()
        {
            try
            {
                Thread.CurrentThread.Name = "SendCommandsService Main Thread";

                // log4 net
                XmlConfigurator.Configure();

                HostFactory.Run(
                    x =>
                    {
                        x.UseLog4Net();
                        x.Service<SendCommandService>(
                            s =>
                            {
                                s.ConstructUsing(name => new SendCommandService());
                                s.WhenStarted(service => service.StartAsync());
                                s.WhenStopped(service => service.Stop());
                            });

                        x.RunAsLocalSystem();
                        x.SetDescription("Service to send commands to the domain service");
                        x.SetDisplayName("Performance SendCommandService");
                        x.SetServiceName("Performance.SendCommandService");
                    });
            }
            catch (Exception e)
            {
                Log.Info("Exception occured..." + e.Message);
            }
        }
    }
}
