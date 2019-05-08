namespace Performance.Library
{
    using System;
    using System.Linq;

    using Voogd.Library.Messaging;

    public enum MessageRetry
    {
        ImmediateRetry,
        RedeliverRetry,
        NoRetry,
        ImmediateError
    }

    public class MessageHandlerException : Exception
    {
        public MessageHandlerException(Command cmd, Exception ex, MessageRetry errorRetry = MessageRetry.RedeliverRetry)
            : base(cmd.ToString(), ex)
        {
            this.Command = cmd;
            this.ErrorRetry = errorRetry;
        }

        public MessageHandlerException(Event evt, Exception ex, MessageRetry errorRetry = MessageRetry.RedeliverRetry)
            : base(evt.ToString(), ex)
        {
            this.Event = evt;
            this.ErrorRetry = errorRetry;
        }

        public MessageHandlerException(Exception ex, MessageRetry errorRetry, int retries = 0, TimeSpan? delay = null)
            : base(ex.ToString(), ex)
        {
            this.ErrorRetry = errorRetry;
            this.Retries = retries;
            this.Delay = delay;
        }

        public static MessageHandlerException GetMessageHandlerException(AggregateException aggregateException)
        {
            var immediateError =
                aggregateException.InnerExceptions.OfType<MessageHandlerException>().FirstOrDefault(e =>
                    e.ErrorRetry == MessageRetry.ImmediateError);
            if (immediateError != null) return immediateError;

            return aggregateException.InnerExceptions.OfType<MessageHandlerException>().OrderByDescending(e => e.Retries).FirstOrDefault();
        }

        public MessageRetry ErrorRetry { get; }

        public Command Command { get; }

        public Event Event { get; }

        public int Retries { get; }

        public TimeSpan? Delay { get; }

    }
}