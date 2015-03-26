namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    using Domain.Interfaces.Configuration;

    public class AzureCloudConfig : IAzureCloudConfig
    {
        private readonly IConfigurationService _configurationService;

        public AzureCloudConfig(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public string StorageConnectionString
        {
            get
            {
                return _configurationService.Get<AzureConfiguration>(AzureConfiguration.ConfigurationName).StorageConnectionString;
            }
        }
    }
}
