namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration
{
    using System.Collections.Generic;

    public class RabbitConfiguration
    {
        public static string RabbitConfigurationName { get { return "RabbitConfiguration"; } }

        public RabbitHost MessagingHost { get; set; }

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

        public ushort PreFetchCount { get; set; }

        public int NodeCount { get; set; }

        public int DefaultQueueWarningLimit { get; set; }

        public IEnumerable<QueueWarningLimit> QueueWarningLimits { get; set; }

        public string ConnectionString
        {
            get
            {
                return
                    string.Format(
                        "host={0};virtualHost={1};username={2};password={3};requestedHeartbeat={4};prefetchcount={5};timeout=30",
                        HostName, VirtualHost, UserName, Password, HeartBeatSeconds, PreFetchCount);
            }
        }
    }

    public class QueueWarningLimit
    {
        public string NameEndsWith { get; set; }

        public int Limit { get; set; }
    }
}
