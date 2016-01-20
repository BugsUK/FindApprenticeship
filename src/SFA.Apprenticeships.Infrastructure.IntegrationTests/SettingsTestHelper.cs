namespace SFA.Apprenticeships.Infrastructure.IntegrationTests
{
    using Common.IoC;
    using Logging.IoC;
    using SFA.Infrastructure.Interfaces;
    using StructureMap;

    public static class SettingsTestHelper
    {
        public static string GetStorageConnectionString()
        {
            var tempContainer = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationMgr = tempContainer.GetInstance<IConfigurationManager>();
            var configurationStorageConnectionString =
                configurationMgr.GetAppSetting<string>("ConfigurationStorageConnectionString");

            return configurationStorageConnectionString;
        }
    }
}