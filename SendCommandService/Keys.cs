namespace Performance.SendCommandsService
{
    /// <summary>
    /// Helper class that creates the windsor container.
    /// </summary>
    public static class Keys
    {
        /// <summary>
        /// Naam van de RabbitMq connection string
        /// </summary>
        public static readonly string RabbitMqHostConnectionString = "RabbitMqHost";

        /// <summary>
        /// Naam van het RabbitMq domain endpoint
        /// </summary>
        public static readonly string RabbitMqDomainEndpoint = "RabbitMqDomainEndpoint";

        /// <summary>
        /// Naam van het RabbitMq readmodel endpoint
        /// </summary>
        public static readonly string RabbitMqReadModelEndpoint = "RabbitMqReadModelEndpoint";

        /// <summary>
        /// RabbitMQ username voor het domein.  
        /// </summary>
        public static readonly string RabbitMqUsername = "RabbitMqUsername";

        /// <summary>
        /// RabbitMQ wachtwoord voor het domein.  
        /// </summary>
        public static readonly string RabbitMqPassword = "RabbitMqPassword";




    }
}