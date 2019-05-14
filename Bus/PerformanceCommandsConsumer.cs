using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Infrastructure.Bus
{
    using MassTransit;

    using Performance.Contracts;

    class PerformanceCommandsConsumer :
        IConsumer<ProcessPerformanceMessage>
    {
        public Task Consume(ConsumeContext<ProcessPerformanceMessage> context)
        {
            throw new NotImplementedException();
        }
    }
}
