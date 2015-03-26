namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System.Collections.Generic;

    public class SearchConfiguration : ElasticsearchConfiguration { }

    public class LogstashConfiguration : ElasticsearchConfiguration { }

    public class ElasticsearchConfiguration
    {
        public const string SearchConfigurationName = "SearchConfiguration";

        public const string LogstashConfigurationName = "LogstashConfiguration";

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
