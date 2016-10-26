namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public static class ScheduledJobQueues
    {
        public const string SavedSearch = "savedsearchscheduler";
        public const string ApplicationEtl = "applicationstatusscheduler";
        public const string VacancyEtl = "vacancyindexscheduler";
        public const string Monitor = "monitorscheduler";
        public const string DailyDigest = "dailydigestscheduler";
        public const string DailyMetrics = "dailymetricsscheduler";
        public const string Housekeeping = "housekeepingscheduler";
        public const string VacancyStatus = "vacancystatusscheduler";
        public const string FaaMigration = "faamigrationscheduler";
    }
}
