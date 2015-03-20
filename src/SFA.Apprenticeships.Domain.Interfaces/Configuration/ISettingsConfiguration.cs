namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface ISettingsConfiguration
    {
        #region Simple Settings

        int VacancyResultsPerPage { get; }

        int LocationResultLimit { get; }

        string PostcodeServiceEndpoint { get; }

        int ActivationCodeExpiryDays { get; }

        int PasswordResetCodeExpiryDays { get; }

        int UnlockCodeExpiryDays { get; }

        int MaximumPasswordAttemptsAllowed { get; }

        bool UseCaching { get; }

        #endregion

        #region Complex Settings

        IMongoConfiguration MongoConfiguration { get; }

        IRabbitConfiguration RabbitConfiguration { get; }

        IElasticsearchConfiguration SearchConfiguration { get; }

        IElasticsearchConfiguration LogstashConfiguration { get; }

        ISearchTermFactors SearchTermFactors { get; }

        IReachSmsConfiguration SmsConfiguration { get; }

        ISendGridConfiguration EmailConfiguration { get; }

        #endregion
    }
}
