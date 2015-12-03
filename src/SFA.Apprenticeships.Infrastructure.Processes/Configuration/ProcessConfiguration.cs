namespace SFA.Apprenticeships.Infrastructure.Processes.Configuration
{
    public class ProcessConfiguration
    {
        public const string ConfigurationName = "ProcessConfiguration";

        public int VacancyAboutToExpireNotificationHours { get; set; }

        public int ApplicationStatusExtractWindow { get; set; }

        public bool EnableVacancyStatusPropagation { get; set; }

        public bool EnableVacancySiteMap { get; set; }

        public bool StrictEtlValidation { get; set; }

        public string VacancyIndexDataProvider { get; set; }
    }
}
