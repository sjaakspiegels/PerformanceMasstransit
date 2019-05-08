using System;
namespace Performance.Library
{
    using Voogd.Library.Messaging;

    /// <summary>
    /// Interface voor sync consumer van een message
    /// </summary>
    public interface IConsumeSync<in T> : IConsume<T> where T : IMessage
    {
        void When(T message);
    }
}

