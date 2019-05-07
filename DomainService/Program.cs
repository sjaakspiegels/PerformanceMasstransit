namespace Performance.DomainService
{
    using System;
    using System.Threading;

    using log4net.Config;

    using Topshelf;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            try
            {
                Thread.CurrentThread.Name = "Domain Service Main Thread";

                // log4 net
                XmlConfigurator.Configure();

                HostFactory.Run(
                    x =>
                    {
                        x.UseLog4Net();
                        x.Service<DomainService>(
                            s =>
                            {
                                s.ConstructUsing(name => new DomainService());
                                s.WhenStarted(service => service.Start());
                                s.WhenStopped(service => service.Stop());
                            });
                        x.RunAsLocalSystem();
                        x.SetDescription("Altas domain service voor converteren van polissen naar Via.");
                        x.SetDisplayName("Domain Service");
                        x.SetServiceName("Domain.Service");
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured..." + e.Message);
            }
        }
    }
}
