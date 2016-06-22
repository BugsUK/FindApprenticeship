using System.Linq;

namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Raa
{
    using Application.ReferenceData;

    using Common.IoC;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using Infrastructure.Raa.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using Repositories.Sql.Configuration;
    using Repositories.Sql.IoC;

    using Application.Candidate.Configuration;

    using SFA.Apprenticeships.Application.Interfaces;
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
