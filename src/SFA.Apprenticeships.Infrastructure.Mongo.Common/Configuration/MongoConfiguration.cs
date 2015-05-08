namespace SFA.Apprenticeships.Infrastructure.Mongo.Common.Configuration
{
    public class MongoConfiguration
    {
        public string CandidatesDb { get; set; }

        public string ApplicationsDb { get; set; }

        public string UsersDb { get; set; }

        public string CommunicationsDb { get; set; }

        public string AdminDb { get; set; }

        public string AuthenticationDb { get; set; }

        public string AuditDb { get; set; }
    }
}
