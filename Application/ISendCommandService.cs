namespace Performance.Application
{
    using System.Threading.Tasks;

    public interface ISendCommandService
    {
        Task SendCommand();

    }
}
