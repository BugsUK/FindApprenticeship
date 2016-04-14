namespace SFA.Apprenticeships.Data.Migrate.Faa.Configuration
{
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class MongoConfiguration
    {
        public string MetricsCandidatesDb { get; set; }

        public string MetricsApplicationsDb { get; set; }

        public string MetricsUsersDb { get; set; }

        public string MetricsCommunicationsDb { get; set; }

        public string MetricsAuditDb { get; set; }
    }
}
