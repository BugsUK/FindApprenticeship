using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Raa
{
    using Application.ReferenceData;
    using Common.Configuration;
    using Common.IoC;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using Infrastructure.Caching.Memory.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.Raa.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using Repositories.Sql.Configuration;
    using Repositories.Sql.IoC;
    using SFA.Infrastructure.Interfaces;
    using StructureMap;

    [TestFixture]
    public class ReferenceDataProviderTests
    {
        private IReferenceDataProvider _referenceDataProvider;

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var sqlConfiguration = configurationService.Get<SqlConfiguration>();

            container = new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry(new RepositoriesRegistry(sqlConfiguration));
                x.AddRegistry(new RaaRegistry(new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Raa }));
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
