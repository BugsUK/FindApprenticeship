namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    using System.Collections.Generic;

    public interface IRabbitConfiguration
    {
        IRabbitHost MessagingHost { get; }
        
        IRabbitHost LoggingHost { get; } 
    }

    public interface IRabbitHost
    {
        string Name { get; }

        string VirtualHost { get; }

        string UserName { get; }

        string Password { get; }

        bool Durable { get; }

        int HeartBeatSeconds { get; }

        string HostName { get; }

        bool OutputEasyNetQLogsToNLogInternal { get; }

        int PreFetchCount { get; }

        int NodeCount { get; }

        int DefaultQueueWarningLimit { get; }

        IEnumerable<IQueueWarningLimit> QueueWarningLimits { get; }
    }

    public interface IQueueWarningLimit
    {
        string NameEndsWith { get; }

        int Limit { get; }
    }
}
