﻿namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.LegacyWebServices
{
    using System.Linq;
    using Application.ReferenceData;
    using Common.Configuration;
    using Common.IoC;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using Infrastructure.Caching.Memory.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ReferenceDataProviderTests
    {
        private IReferenceDataProvider _referenceDataProvider;

        [SetUp]
        public void SetUp()
        {
            var configurationStorageConnectionString = SettingsTestHelper.GetStorageConnectionString();

            var container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(new CacheConfiguration(), configurationStorageConnectionString));
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Legacy }));
            });
            
            _referenceDataProvider = container.GetInstance<IReferenceDataProvider>();
        }

        [Test, Category("Integration")]
        public void ReturnsCategoryDataFromFrameworksService()
        {
            var categories = _referenceDataProvider.GetCategories();
            categories.Count().Should().BeGreaterThan(0);

            foreach (Category category in categories)
            {
                category.SubCategories.Count().Should().BeGreaterThan(0);
            }
        }

    }
}
