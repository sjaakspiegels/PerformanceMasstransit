namespace PerformanceConsole
{
    using System.Threading.Tasks;

    using MassTransit;

    using Voogd.Library.Messaging;

    public class MessageHandler : Handler, IConsumer<IMyMessage>
    {
        public Task Consume(ConsumeContext<IMyMessage> context)
        {
            return Task.FromResult(0);
        }
    }

}
