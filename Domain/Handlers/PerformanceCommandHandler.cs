namespace Performance.Domain.Handlers
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using GreenPipes.Util;

    using Performance.Contracts;
    using Performance.Library;

    using Voogd.Library.Messaging;

    using MessageRetry = Performance.Library.MessageRetry;

    public class PerformanceCommandHandler : Handler, 
        IPerformanceTesterApplicationService,
        Library.IConsumeSync<ProcessPerformanceMessage>, Voogd.Library.Messaging.IConsume<ProcessPerformanceMessage>
    {
        private readonly IWorkRepository repository;

        private static readonly Random Rnd = new Random();

        public PerformanceCommandHandler(IWorkRepository repository)
        {
            this.repository = repository;
        }

        //public Task When(ProcessPerformanceMessage message)
        //{
        //    if (Rnd.Next(100) < 10)
        //    {
        //        throw new Library.MessageHandlerException(new Exception("Bewuste fout"),
        //            MessageRetry.RedeliverRetry, 2);
        //    }

        //    this.repository.DoWork();

        //    return TaskUtil.Completed;
        //}

        public void When(ProcessPerformanceMessage message)
        {
            if (Rnd.Next(100) < 10)
            {
                throw new Library.MessageHandlerException(new Exception("Bewuste fout"),
                    MessageRetry.RedeliverRetry, 2, TimeSpan.FromSeconds(20));
            }
            this.repository.DoWork();
        }
    }
}
