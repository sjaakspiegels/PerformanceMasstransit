

namespace Performance.Application.Installers
{
    using System.Configuration;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using MassTransit;

    using Performance.SendCommandsService;

    public class SendCommandServiceInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Registreert de implementatie voor de polisconversie verwerking.
        /// Voor nu wordt de Xml versie gebruikt die de berichten met Adn formaat verwerkt
        /// </summary>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var rabbitMqHost = ConfigurationManager.ConnectionStrings[Keys.RabbitMqHostConnectionString]
                .ConnectionString;
            var rabbitDomainEndpoint = ConfigurationManager.AppSettings[Keys.RabbitMqDomainEndpoint];
            var commandoEndpoint = rabbitMqHost + '/' + rabbitDomainEndpoint;

            //De refactored -meer generieke- registraties voor de bestandsconversie
            container.Register(Component.For<ISendCommandService>()
                .ImplementedBy<SendCommandService>()
                .DependsOn(Dependency.OnValue("commandoEndpoint", commandoEndpoint))
                .LifestyleTransient());

            container.Register(Component.For<IWindsorContainer>().Instance(container));

            container.Register(Component.For<Voogd.Library.Messaging.IBus>()
                .UsingFactoryMethod((k, c) => new Voogd.Library.Messaging.MassTransit.Bus(k.Resolve<IBusControl>()))
                .LifeStyle.Singleton);
        }
    }
}