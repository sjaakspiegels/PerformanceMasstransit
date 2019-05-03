namespace Application
{
    using System;

    using Voogd.Library.Messaging;


    public class SendCommandsService : ISendCommandsService
    {
        /// <summary>
        /// De bus
        /// </summary>
        private readonly IBus bus;

        /// <summary>
        /// Het endpoint adres voor de commando's
        /// </summary>
        private readonly string endpoint;



        /// <summary>
        /// Initialiseert een nieuwe instantie van de KlantSynchronisatieService class
        /// </summary>
        public SendCommandsService(IBus bus, string endpoint)
        {
            this.bus = bus;
            this.endpoint = endpoint;
        }


        public void StartSendingCommands(int numberOfCommands, int messagesPerSeconde)
        {
            throw new NotImplementedException();
        }
    }
}
