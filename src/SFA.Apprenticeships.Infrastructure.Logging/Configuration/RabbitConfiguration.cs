namespace SFA.Apprenticeships.Infrastructure.Logging.Configuration
{
    public class RabbitConfiguration
    {
        public RabbitHost LoggingHost { get; set; } 
    }

    public class RabbitHost
    {
        public string VirtualHost { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Durable { get; set; }

        public ushort HeartBeatSeconds { get; set; }

        public string HostName { get; set; }

        public ushort Port { get; set; }

        public bool OutputEasyNetQLogsToNLogInternal { get; set; }
    }
}
