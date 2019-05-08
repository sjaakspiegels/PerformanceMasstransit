namespace Performance.Library
{
    using System;
    using System.Configuration;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using global::MassTransit;

    using GreenPipes;

    using Performance.Contracts;
    using Performance.Domain;
    using Performance.Domain.Handlers;

    using Voogd.Library.Extensions;
    using Voogd.Library.Messaging.MassTransit;

    using AggregateException = Voogd.Library.Exceptions.AggregateException;

    /// <summary>
    /// Installs the service bus into the container.
    /// </summary>
    public class ServiceBusInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Installeert de bus
        /// </summary>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var workRepository = container.Resolve<IWorkRepository>();
            var consumer = container.Resolve<PerformanceCommandHandler>();

            var busControl = global::MassTransit.Bus.Factory.CreateUsingRabbitMq(
                sbc =>
                {
                    // Host control
                    var host =
                        sbc.Host(
                            new Uri(ConnectionString(ConfigKeys.RabbitMqHostConnectionString)),
                            h =>
                            {
                                h.Username(AppSetting(ConfigKeys.RabbitMqUsername));
                                h.Password(AppSetting(ConfigKeys.RabbitMqPassword));
                                //h.Heartbeat(AppSetting<ushort>(ConfigKeys.RabbitMqHeartbeat, 60));
                                //h.PublisherConfirmation = false;

                                //// Cluster configuraties (indien van toepassing)
                                //if (!string.IsNullOrEmpty(AppSetting(ConfigKeys.RabbitMqHostClusterMembers)))
                                //{
                                //    h.UseCluster(cluster =>
                                //    {
                                //        string[] hostnames = AppSetting(ConfigKeys.RabbitMqHostClusterMembers).Split(';');
                                //        cluster.ClusterMembers = hostnames;
                                //    });
                                //}
                            });

//                    sbc.UseLog4Net();

                    // Receive endpoint
                    if (!string.IsNullOrEmpty(AppSetting(ConfigKeys.RabbitMqReceiveEndpoint)))
                    {
                        sbc.ReceiveEndpoint(
                            host,
                            AppSetting(ConfigKeys.RabbitMqReceiveEndpoint),
                            ep =>
                            {
  //                              ep.PrefetchCount = AppSetting<ushort>(ConfigKeys.MassTransitPrefetchCount, 32);
  //                              ep.UseConcurrencyLimit(AppSetting(ConfigKeys.MassTransitConcurrencyLimit, 8));

                                // Retry mechanisme
                                //ep.UseRetry(r =>
                                //{
                                //    r.Incremental(
                                //        AppSetting(ConfigKeys.MassTransitRetryLimit, 3),
                                //        TimeSpan.FromSeconds(
                                //            AppSetting(ConfigKeys.MassTransitRetryInitialWaitInSeconds, 0.5)),
                                //        TimeSpan.FromSeconds(
                                //            AppSetting(ConfigKeys.MassTransitRetryIntervalIncrementInSeconds, 1)));
                                //    r.Ignore<AggregateException>(x => !x.Retry);
                                //});

                                //// Rate limit (indien van toepassing
                                //if (!string.IsNullOrEmpty(AppSetting(ConfigKeys.MassTransitRateLimit)))
                                //{
                                //    ep.UseRateLimit(
                                //        AppSetting<int>(ConfigKeys.MassTransitRateLimit, 100),
                                //        TimeSpan.FromSeconds(AppSetting(ConfigKeys.MassTransitRateIntervalInSeconds, 1)));
                                //}
                                ep.UseMessageRetry(r => r.Immediate(5));
//                                ep.Consumer(() => consumer);
                            });
                    }
                });

            container.Register(Component.For<IBusControl>().Instance(busControl));

            busControl.Start();
        }

        /// <summary>
        /// Korte methode om app setting op te halen
        /// </summary>
        private static string AppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// Korte methode om app setting op te halen
        /// </summary>
        private static T AppSetting<T>(string name, T defaultValue)
        {
            return ConfigurationManager.AppSettings.Get<T>(name, defaultValue);
        }

        /// <summary>
        /// Korte methode om connection strnig setting op te halen
        /// </summary>
        private static string ConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}