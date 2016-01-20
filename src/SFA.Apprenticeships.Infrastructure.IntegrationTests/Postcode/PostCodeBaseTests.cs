namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Postcode
{
    using Common.Configuration;
    using Common.IoC;
    using Infrastructure.Postcode.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using StructureMap;

    public class PostCodeBaseTests
    {
        protected Container Container;

        [SetUp]
        public void Setup()
        {
            var tempContainer = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationMgr = tempContainer.GetInstance<IConfigurationManager>();
            var configurationStorageConnectionString =
                configurationMgr.GetAppSetting<string>("ConfigurationStorageConnectionString");

            Container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(new CacheConfiguration(), configurationStorageConnectionString));
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<PostcodeRegistry>();
            });
        }
    }
}