namespace SFA.Apprenticeships.Data.Migrate.Faa.Configuration
{
    public class MigrateFromFaaToAvmsPlusConfiguration
    {
        public string SourceConnectionString { get; set; }

        public string TargetConnectionString { get; set; }

        public string SourceCandidatesDb { get; set; }

        public string SourceApplicationsDb { get; set; }

        public string SourceUsersDb { get; set; }

        public string SourceAuditDb { get; set; }

        public string TargetCandidatesDb { get; set; }

        public string TargetApplicationsDb { get; set; }

        public string TargetUsersDb { get; set; }

        public bool AnonymiseData { get; set; }

        public bool IsEnabled { get; set; }

        public int SleepTimeBetweenCyclesInSeconds { get; set; }

        public bool MigrateCandidates { get; set; }

        public bool MigrateApplications { get; set; }
    }
}
