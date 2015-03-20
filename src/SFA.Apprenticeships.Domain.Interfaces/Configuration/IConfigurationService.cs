namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    using Entities.Configuration;

    public interface IConfigurationService
    {
        void ReloadConfiguration();

        SettingsConfiguration GetSettingsConfiguration();

        //DISCUSS: May just need GetSettingsConfiguration above.

        MongoConfiguration GetMongoConfiguration();

        RabbitConfiguration GetRabbitConfiguration();

        ElasticsearchConfiguration GetSearchConfiguration();

        SearchFactorConfiguration GetSearchFactorConfiguration();

        ElasticsearchConfiguration GetLogstashConfiguration();

        ReachSmsConfiguration GetSmsConfiguration();

        SendGridConfiguration GetEmailConfiguration();
    }
}
