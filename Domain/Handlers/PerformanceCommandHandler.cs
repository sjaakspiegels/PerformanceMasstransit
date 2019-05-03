namespace Performance.Domain.Handlers
{
    using Performance.Contratcs;

    using Voogd.Library.Messaging;

    public class PerformanceCommandHandler : Handler, IPerformanceTesterApplicationService,
        IConsume<ProcessPerformanceMessage>
    {
        private readonly IWorkRepository repository;

        private readonly int delay;

        public PerformanceCommandHandler(IWorkRepository repository, int delay)
        {
            this.repository = repository;
            this.delay = delay;
        }

        public void When(ProcessPerformanceMessage message)
        {
            this.repository.DoWork(this.delay);
        }
    }
}
