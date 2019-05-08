namespace Performance.Library
{

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using global::MassTransit.WindsorIntegration;
    using System.Linq;

    /// <summary>
    /// Installer class voor de unit of work
    /// </summary>
    public class ConsumerInstaller<T> : IWindsorInstaller
    {
        /// <summary>
        /// Registreert de verschillende consumers
        /// </summary>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var consumerType = typeof(T);
            container.Register(Component.For(consumerType).ImplementedBy(consumerType).LifeStyle.Scoped<MessageScope>());

            // Bepaalde de verschillende IConsume interfaces die op de consumer zijn geimplementeerd
            var all = consumerType.GetInterfaces();
            var interfaceTypes = consumerType.GetInterfaces()
                .Where(y => y.IsGenericType &&
                            (y.GetGenericTypeDefinition() == typeof(IConsume<>))).ToList();

            foreach (var interfaceType in interfaceTypes)
            {
                var messageType = interfaceType.GetGenericArguments().First();

                // Genereer voor elke interface een consumer type
                var consumer = typeof(Consumer<,>).MakeGenericType(messageType, consumerType);
                container.Register(Component.For(consumer).LifeStyle.Scoped<MessageScope>());
            }
        }
    }
}