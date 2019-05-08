using System;
namespace Performance.Library
{
    using System.Threading.Tasks;

    using Voogd.Library.Messaging;

    /// <summary>
    /// Interface voor async consumer van een message
    /// De verwerking van een message wordt uitgevoerd in een task
    /// </summary>
    public interface IConsumeAsync<in T> : Library.IConsume<T> where T : IMessage
    {
        Task When(T message);
    }
}
