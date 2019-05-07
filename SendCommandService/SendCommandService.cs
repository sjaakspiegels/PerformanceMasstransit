namespace Performance.SendCommandsService
{
    using System;
    using System.Diagnostics;
    using System.ServiceProcess;
    using System.Threading;
    using System.Threading.Tasks;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using MassTransit;

    using Performance.Application;
    using Performance.Application.Installers;

    using Topshelf.Logging;

    using Voogd.Library.Messaging.MassTransit;

    public partial class SendCommandService : ServiceBase
    {
        private static readonly LogWriter Log = HostLogger.Get<SendCommandService>();

        private ISendCommandService sendCommandService;

        private bool stopped;

        private Thread verwerkenThread;

        private IWindsorContainer container;

        private IBusControl bus;

        public SendCommandService()
        {
            this.stopped = false;
            this.verwerkenThread = null;

            this.InitializeComponent();
        }

        public async Task StartAsync()
        {
            // De service is gestart.
            this.stopped = false;

            Log.Info("Send commands Service starting...");

            // Dependecy injection
            this.container = new WindsorContainer().Install(
                new ServiceBusInstaller(),
                new SendCommandServiceInstaller());

            this.container.BeginScope();
            this.sendCommandService = this.container.Resolve<ISendCommandService>();

            this.bus = this.container.Resolve<IBusControl>();
            
            // Een losse thread aanmaken en starten om de verwerkingThread buitenom de main thread te draaien.
//            this.verwerkenThread = new Thread(this.StartVerwerkenThread);
//            this.verwerkenThread.Start();

            await this.DoSending();

            Log.Info("Send commands Service started");
        }

        private Task DoSending()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    using (this.container.BeginScope())
                    {
                        await this.sendCommandService.SendCommand();
                    }
                }
            });
        }

        /// <summary>
        /// Starten van de Verwerken service vanuit de Workerthread i.p.v. rechtstreeks uit de main thread.
        /// </summary>
        private void StartVerwerkenThread()
        {
            while (this.verwerkenThread.IsAlive && !this.stopped)
            {
                try
                {
                    using (this.container.BeginScope())
                    {
                        this.sendCommandService.SendCommand();
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Fout bij .", e);
                }

            }
        }

        public new void Stop()
        {
            Log.Info("Send commands Service stopping...");

            // De service is stopping.
            this.stopped = true;

            this.bus.Stop();

            this.container.Dispose();

            Log.Info("Send commands Service stopped.");

        }

    }
}
