namespace Performance.Infrastructure.Bus
{
    using System.Linq;

    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using MassTransit;

    using Performance.Domain.Handlers;

    using Voogd.Library.DomainBase.Dispatcher;
    using Voogd.Library.DomainBase.Messaging.MassTransit;

    public class DomainBusInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Registreert de handlers
        /// </summary>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Voogd.Library.Messaging.IBus>()
                    .UsingFactoryMethod((k, c) => new CommitDispatcher(k.Resolve<IBusControl>()))
                    .Forward<IDispatchCommits>()
                    .LifeStyle.Singleton);

            // for factory
            if (container.Kernel.GetFacilities().All(x => x.GetType() != typeof(TypedFactoryFacility)))
            {
                container.AddFacility<TypedFactoryFacility>();
            }

            // Installeert de commandhandler in de bus
            container.Install(new Library.ConsumerInstaller<PerformanceCommandHandler>());
        }
    }
}
