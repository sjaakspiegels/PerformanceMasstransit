namespace Performance.Domain.Handlers
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using Performance.Contracts;
    using Performance.Library;

    using Voogd.Library.Messaging;

    using MessageRetry = Performance.Library.MessageRetry;

    public class PerformanceCommandHandler : Handler, 
        IPerformanceTesterApplicationService,
        Library.IConsumeAsync<ProcessPerformanceMessage>
    {
        private readonly IWorkRepository repository;

        private static Random rnd = new Random();

        public PerformanceCommandHandler(IWorkRepository repository)
        {
            this.repository = repository;
        }

        public Task When(ProcessPerformanceMessage message)
        {
            return Task.Run(() =>
            {
                if (rnd.Next(100) < 5)
                {
                    throw new Library.MessageHandlerException(new Exception("Bewuste fout"),
                        MessageRetry.RedeliverRetry);
                }

                this.repository.DoWork();
            });
        }

        //public void When(ProcessPerformanceMessage message)
        //{
        //    //if (rnd.Next(100) < 5)
        //    //{
        //    //    throw new Library.MessageHandlerException(new Exception("Bewuste fout"), MessageRetry.ImmediateError);
        //    //}
        //    this.repository.DoWork();
        //}
    }
}
