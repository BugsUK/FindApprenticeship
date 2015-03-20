namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface IConfigurationService
    {
        void ReloadConfiguration();

        ISettingsConfiguration GetSettingsConfiguration();

        //DISCUSS: May just need GetSettingsConfiguration above.

        IMongoConfiguration GetMongoConfiguration();

        IRabbitConfiguration GetRabbitConfiguration();

        IElasticsearchConfiguration GetSearchConfiguration();

        ISearchFactorConfiguration GetSearchFactorConfiguration();

        IElasticsearchConfiguration GetLogstashConfiguration();

        IReachSmsConfiguration GetSmsConfiguration();

        ISendGridConfiguration GetEmailConfiguration();
    }
}
