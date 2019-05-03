namespace PerformanceConsole
{
    using System;
    using System.Net;
    using System.Runtime.Remoting.Contexts;
    using System.Threading.Tasks;

    using MassTransit;

    using Voogd.Library.Messaging;

    public class MessageHandler : Handler, IConsumer<IMyMessage>
    {
        private static readonly Random  random = new Random();

        public async Task Consume(ConsumeContext<IMyMessage> context)
        {
          //  Task.Yield();
           // Console.WriteLine("Consumed");

           if (random.Next(10) < 6)
           {
               var rety = context.GetRetryAttempt();
               if (rety > 4)
               {
                   throw new Exception($"Er gaat iets fout met message {context.Message.Number}");
               }
               await context.Defer(TimeSpan.FromSeconds(2));
           }
        }
    }
}

