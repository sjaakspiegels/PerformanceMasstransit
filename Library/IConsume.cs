namespace Performance.Library
{
    using Voogd.Library.Messaging;

    /// <summary>
    /// Base class voor het consumeren van een message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConsume<in T> where T : IMessage
    {
    }
}