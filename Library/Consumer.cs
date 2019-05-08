namespace Performance.Library
{
    using global::MassTransit;
    using global::Voogd.Library.Messaging;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Deze consumer klasse gebruik je om je op een bepaalde 
    /// message te abonneren bij Masstransit
    /// </summary>
    public class Consumer<TMessage, TConsumer> : IConsumer<TMessage>
        where TMessage : class, IMessage
        where TConsumer : IConsume<TMessage>
    {
        /// <summary>
        /// De consumer / handler van een message
        /// </summary>
        private readonly TConsumer consumer;

        /// <summary>
        /// Initialiseert een nieuwe instantie van de Consumer class.
        /// </summary>
        public Consumer(TConsumer consumer)
        {
            this.consumer = consumer;
        }

        /// <summary>
        /// Verwerkt de message als die ontvangen wordt vanaf de bus.
        /// </summary>
        public Task Consume(ConsumeContext<TMessage> context)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (this.consumer is IConsumeAsync<TMessage> @async)
                    {
                        await @async.When(context.Message);
                    }
                    else
                    {
                        (this.consumer as IConsumeSync<TMessage>)?.When(context.Message);
                    }
                }
                catch (MessageHandlerException e)
                {
                    await this.HandleMessageHandlerException(context, e);
                }
                catch (AggregateException e)
                {
                    var exception = MessageHandlerException.GetMessageHandlerException(e);
                    if (exception != null)
                    {
                        await this.HandleMessageHandlerException(context, exception);
                    }
                    else
                    {
                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// Afhandelen van een messagehandler exception
        /// </summary>
        /// <param name="context"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task HandleMessageHandlerException(ConsumeContext<TMessage> context, MessageHandlerException e)
        {
            switch (e.ErrorRetry)
            {
                case MessageRetry.RedeliverRetry when this.RetryCount(context) < e.Retries:
                    await context.Defer(e.Delay ?? TimeSpan.FromSeconds(1));
                    break;
                case MessageRetry.ImmediateRetry when this.RetryCount(context) < e.Retries:
                    await context.Defer(TimeSpan.FromMilliseconds(1));
                    break;
                case MessageRetry.NoRetry:
                    // No retry. Laat de message verdwijnen
                    break;
                case MessageRetry.ImmediateError:
                    // ImmediateError. Zet de message direct op de error queue
                    throw e;
                default:
                    throw e;
            }
        }

        /// <summary>
        /// Retry count van de context
        /// </summary>
        private int RetryCount(ConsumeContext<TMessage> context)
        {
            if (context == null)
            {
                return 0;
            }

            if (context.Headers.TryGetHeader("MT-Redelivery-Count", out var value))
            {
                return Convert.ToInt32(value);
            }

            return 0;
        }
    }
}

