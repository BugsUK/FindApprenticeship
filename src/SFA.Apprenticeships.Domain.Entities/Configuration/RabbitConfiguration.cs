namespace SFA.Apprenticeships.Domain.Entities.Configuration
{
    using System.Collections.Generic;

    public class RabbitConfiguration
    {
        public RabbitHost MessagingHost { get; set; }

        public RabbitHost LoggingHost { get; set; } 
    }

    public class RabbitHost
    {
        public string VirtualHost { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Durable { get; set; }

        public int HeartBeatSeconds { get; set; }

        public string HostName { get; set; }

        public bool OutputEasyNetQLogsToNLogInternal { get; set; }

        public int PreFetchCount { get; set; }

        public int NodeCount { get; set; }

        public int DefaultQueueWarningLimit { get; set; }

        public IEnumerable<QueueWarningLimit> QueueWarningLimits { get; set; }
    }

    public class QueueWarningLimit
    {
        public string NameEndsWith { get; set; }

        public int Limit { get; set; }
    }
}
