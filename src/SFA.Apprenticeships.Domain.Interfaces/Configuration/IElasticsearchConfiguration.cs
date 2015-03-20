namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    using System;
    using System.Collections.Generic;

    public interface IElasticsearchConfiguration
    {
        string HostName { get; }

        int NodeCount { get; }

        int Timeout { get; }

        IEnumerable<IElasticsearchIndex> Indexes { get; } 
    }

    public interface IElasticsearchIndex
    {
        string Name { get; }

        Type MappingType { get;  }
    }
}
