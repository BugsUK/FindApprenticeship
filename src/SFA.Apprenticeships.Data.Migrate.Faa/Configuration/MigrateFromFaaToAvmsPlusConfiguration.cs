namespace SFA.Apprenticeships.Data.Migrate.Faa.Configuration
{
    public class MigrateFromFaaToAvmsPlusConfiguration
    {
        public string SourceConnectionString { get; set; }

        public string TargetConnectionString { get; set; }

        public string CandidatesDb { get; set; }

        public string ApplicationsDb { get; set; }

        public string UsersDb { get; set; }

        public bool AnonymiseData { get; set; }

        public bool IsEnabled { get; set; }
    }
}
