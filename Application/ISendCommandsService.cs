namespace Application
{
    public interface ISendCommandsService
    {
        void StartSendingCommands(int numberOfCommands, int messagesPerSeconde);

    }
}
