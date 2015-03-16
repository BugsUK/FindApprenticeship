namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    using Domain.Interfaces.Configuration;

    public class AzureConsoleCloudConfig : IAzureCloudConfig
    {
        public AzureConsoleCloudConfig(IConfigurationManager configurationManager)
        {
            StorageConnectionString = configurationManager.GetAppSetting<string>("StorageConnectionString");
        }

        public string StorageConnectionString { get; private set; }
    }
}
