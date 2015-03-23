namespace SFA.Apprenticeships.Infrastructure.Common.UnitTests.Configuration
{
    using System;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Caching.Memory;
    using Common.Configuration;
    using Domain.Entities.Configuration;
    using Domain.Interfaces.Caching;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationServiceTests
    {
        //[Test]
        //public void ShouldReturn()
        //{
        //    var configurationCacheKey = new ConfigurationCacheKey();
        //    var configManager = new Mock<IConfigurationManager>();
        //    var logService = new Mock<ILogService>();
        //    var cacheService = new MemoryCacheService(logService.Object);

        //    var configurationService = new ConfigurationService(configManager.Object, logService.Object, cacheService);
        //    var config = configurationService.Get<string>("test");
        //}

        ////[Test]
        //public void ShouldFallBackToFileSystemConfigurationFile()
        //{
        //    var configManager = new Mock<IConfigurationManager>();
        //    var logService = new Mock<ILogService>();
        //    var cacheService = new Mock<ICacheService>();

        //    cacheService.Setup(x => x.Get(It.IsAny<ConfigurationCacheKey>(), It.IsAny<Func<SettingsConfiguration>>));

        //    var configurationService = new ConfigurationService(configManager.Object, logService.Object, cacheService.Object);
            
        //    configurationService.GetSettingsConfiguration();
        //    //cacheService.Verify(x => x.Get(It.IsAny<ConfigurationCacheKey>(), It.IsAny<Func<SettingsConfiguration>>), Times.Exactly(2));
        //}

        //[Test]
        //public void ShouldDeserialiseConfigurationFileFromFileSystem()
        //{
        //    var configurationCacheKey = new ConfigurationCacheKey();
        //    var configManager = new Mock<IConfigurationManager>();
        //    var logService = new Mock<ILogService>();
        //    var cacheService = new MemoryCacheService(logService.Object);

        //    var configurationService = new ConfigurationService(configManager.Object, logService.Object, cacheService);

        //    //TODO: Need to revert commit bad487be8060314096a9c905cee930026113f2a2
        //    //Not sure why Dave did it, will ask.
        //    //cacheService.Remove(configurationCacheKey.Key());

        //    var config = configurationService.GetSettingsConfiguration();
        //    config.Should().NotBeNull();
        //    config.MongoConfiguration.Should().NotBeNull();
        //    config.RabbitConfiguration.MessagingHost.Should().NotBeNull();
        //    config.RabbitConfiguration.MessagingHost.QueueWarningLimits.Should().NotBeNull();
        //    config.RabbitConfiguration.MessagingHost.QueueWarningLimits.Count().Should().BeGreaterThan(0);
        //    config.RabbitConfiguration.LoggingHost.Should().NotBeNull();
        //    config.SearchConfiguration.Indexes.Should().NotBeNull();
        //    config.SearchConfiguration.Indexes.Count().Should().BeGreaterThan(0);
        //    config.LogstashConfiguration.Should().NotBeNull();
        //    config.LogstashConfiguration.Should().NotBeNull();
        //    config.SearchTermFactors.Should().NotBeNull();
        //    config.SmsConfiguration.Should().NotBeNull();
        //    config.SmsConfiguration.Templates.Should().NotBeNull();
        //    config.SmsConfiguration.Templates.Count().Should().BeGreaterThan(0);
        //    config.EmailConfiguration.Should().NotBeNull();
        //    config.EmailConfiguration.Templates.Should().NotBeNull();
        //    config.EmailConfiguration.Templates.Count().Should().BeGreaterThan(0);
            
        //}
    }
}
