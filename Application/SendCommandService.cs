namespace Performance.Application
{
    using System;
    using System.Threading.Tasks;

    using Performance.Contratcs;

    using Voogd.Library.Messaging;

    public class SendCommandService : ISendCommandService
    {
        /// <summary>
        /// De bus
        /// </summary>
        private readonly IBus bus;

        /// <summary>
        /// Het endpoint adres voor de commando's
        /// </summary>
        private readonly string commandoEndpoint;

        public SendCommandService(IBus bus, string commandoEndpoint)
        {
            this.bus = bus;
            this.commandoEndpoint = commandoEndpoint;
        }


        public Task SendCommand()
        {
            return this.bus.Send(
                new ProcessPerformanceMessage(Guid.NewGuid(), new PerformanceMessage("TestBericht", DateTime.Now)),
                this.commandoEndpoint);
        }
    }
}
