namespace PerformanceConsole
{
    using System.Runtime.Serialization;

    public interface IMyMessage
    {
        [DataMember(Order = 1)] int Number { get; set; }
        [DataMember(Order = 2)] string Description { get; set; }


    }
}
