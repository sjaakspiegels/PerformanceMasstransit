namespace Performance.Domain.Handlers
{
    using Performance.Contratcs;

    using Voogd.Library.Messaging;

    public class PerformanceCommandHandler : Handler, IPerformanceTesterApplicationService,
        IConsume<ProcessPerformanceMessage>
    {
        private readonly IWorkRepository repository;

        private readonly int delay;

        public PerformanceCommandHandler(IWorkRepository repository)
        {
            this.repository = repository;
            this.delay = 10;
        }

        public void When(ProcessPerformanceMessage message)
        {
            this.repository.DoWork(this.delay);
        }
    }
}
