namespace Performance.Infrastructure.Bus
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using GreenPipes;
    using GreenPipes.Internals.Extensions;

    using MassTransit;
    using MassTransit.Saga;
    using MassTransit.Util;
    using MassTransit.WindsorIntegration;

    using Performance.Contracts;
    using Performance.Domain;
    using Performance.Domain.Handlers;

    using Voogd.Library.Extensions;
    using Voogd.Library.Messaging;
    using Voogd.Library.Messaging.MassTransit;

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
//              var commandHandler = container.Resolve<PerformanceCommandHandler>();
//              var consumer = new Library.Consumer<ProcessPerformanceMessage, PerformanceCommandHandler>(commandHandler);

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
                            });

                    sbc.UseDelayedExchangeMessageScheduler();
                    // Receive endpoint
                    if (!string.IsNullOrEmpty(AppSetting(ConfigKeys.RabbitMqReceiveEndpoint)))
                    {
                        sbc.ReceiveEndpoint(
                            host,
                            AppSetting(ConfigKeys.RabbitMqReceiveEndpoint),
                            ep =>
                            {
                                ep.PrefetchCount = 100;
                                ep.UseConcurrencyLimit(50);

                               //ep.Consumer(() => consumer);
                               ep.UseMessageScope();
                               ep.LoadFrom(container);
                            });

                    }
                });

            container.Register(Component.For<IBusControl>().Instance(busControl));
            TaskUtil.Await(() =>busControl.StartAsync());
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