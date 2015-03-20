namespace SFA.Apprenticeships.Domain.Entities.Configuration
{
    public class SettingsConfiguration
    {
        #region Simple Settings

        public int VacancyResultsPerPage { get; set; }

        public int LocationResultLimit { get; set; }

        public string PostcodeServiceEndpoint { get; set; }

        public int ActivationCodeExpiryDays { get; set; }

        public int PasswordResetCodeExpiryDays { get; set; }

        public int UnlockCodeExpiryDays { get; set; }

        public int MaximumPasswordAttemptsAllowed { get; set; }

        public bool UseCaching { get; set; }

        #endregion

        #region Complex Settings

        public MongoConfiguration MongoConfiguration { get; set; }

        public RabbitConfiguration RabbitConfiguration { get; set; }

        public ElasticsearchConfiguration SearchConfiguration { get; set; }

        public ElasticsearchConfiguration LogstashConfiguration { get; set; }

        public SearchTermFactors SearchTermFactors { get; set; }

        public ReachSmsConfiguration SmsConfiguration { get; set; }

        public SendGridConfiguration EmailConfiguration { get; set; }

        #endregion
    }
}
