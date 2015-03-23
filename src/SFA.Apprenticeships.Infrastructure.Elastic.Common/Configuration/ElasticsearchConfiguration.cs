namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System.Collections.Generic;

    public class ElasticsearchConfiguration
    {
        public static string SearchConfigurationName { get { return "SearchConfiguration"; } }

        public static string LogstashConfigurationName { get { return "LogstashConfiguration"; } }

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
