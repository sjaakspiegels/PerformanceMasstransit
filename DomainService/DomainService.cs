namespace Performance.DomainService
{
    using System.ServiceProcess;

    using Castle.Facilities.Logging;
    using Castle.MicroKernel.Registration;
    using Castle.Services.Logging.Log4netIntegration;
    using Castle.Windsor;

    using Infrastructure.Bus;
    using Infrastructure.NHibernate;

    using MassTransit;

    using Topshelf.Logging;

    using Voogd.Library.Messaging.MassTransit;

    public partial class DomainService : ServiceBase
    {
        /// <summary>
        /// Log4net acces
        /// </summary>
        private static readonly LogWriter Log = HostLogger.Get<DomainService>();

        /// <summary>
        /// Windsor DI container
        /// </summary>
        private IWindsorContainer container;

        /// <summary>
        /// De bus
        /// </summary>
        private IBusControl bus;

        public DomainService()
        {
            this.InitializeComponent();
        }

        public void Start()
        {
            Log.Info("Domein Windows Service starting...");

            // Dependecy injection
            this.container = new WindsorContainer().Install(
                new NHibernateInstaller(),
                new DomainBusInstaller(),
                new ServiceBusInstaller());

            this.container.AddFacility<LoggingFacility>(f => f.LogUsing<Log4netFactory>());
            this.container.Register(Component.For<IWindsorContainer>().Instance(this.container));

            this.bus = this.container.Resolve<IBusControl>();

            this.bus.Start();
            Log.Info("Domein Service started");
        }

        public new void Stop()
        {
            this.bus.Stop();
            Log.Info("Domein Service stopped");
            this.container.Release(this.bus);
            this.container.Dispose();
        }
    }
}
