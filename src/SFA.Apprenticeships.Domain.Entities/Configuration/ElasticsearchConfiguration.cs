namespace SFA.Apprenticeships.Domain.Entities.Configuration
{
    using System;
    using System.Collections.Generic;

    public class ElasticsearchConfiguration
    {
        public string HostName { get; set; }

        public int NodeCount { get; set; }

        public int Timeout { get; set; }

        public IEnumerable<ElasticsearchIndex> Indexes { get; set; } 
    }

    public class ElasticsearchIndex
    {
        public string Name { get; set; }

        public string MappingType { get; set; }
    }
}
