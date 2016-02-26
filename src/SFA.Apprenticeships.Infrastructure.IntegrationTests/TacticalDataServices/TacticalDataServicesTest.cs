using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.TacticalDataServices
{
    using System.Linq;
    using Application.Organisation;
    using Application.ReferenceData;
    using Common.IoC;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using Infrastructure.Caching.Memory.IoC;
    using Infrastructure.TacticalDataServices.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class TacticalDataServices
    {
        private IReferenceDataProvider _referenceDataProvider;
        private ILegacyProviderProvider _legacyProviderProvider;

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry<TacticalDataServicesRegistry>();
            });

            _referenceDataProvider = container.GetInstance<IReferenceDataProvider>();
            _legacyProviderProvider = container.GetInstance<ILegacyProviderProvider>();
        }

        [Test, Category("Integration")]
        public void ReturnsCategoryDataFromFrameworksService()
        {
            var categories = _referenceDataProvider.GetCategories().ToList();
            categories.Count.Should().BeGreaterThan(0);

            foreach (Category category in categories)
            {
                category.SubCategories.Count().Should().BeGreaterThan(0);
            }
        }

        [Test, Category("Integration")]
        public void ReturnsStandardsDataFromFrameworksService()
        {
            var sectors = _referenceDataProvider.GetSectors().ToList();
            sectors.Count.Should().BeGreaterThan(0);

            foreach (Sector sector in sectors)
            {
                sector.Standards.Count().Should().BeGreaterThan(0);
            }
        }
        
        [TestCase(902763946), Category("Integration")]
        public void ReturnsProviderSiteEmployerLinks(int providerSiteId)
        {
            //Arrange

            //Act
            var links = _legacyProviderProvider.GetVacancyParties(providerSiteId);

            //Assert
            links.Count().Should().Be(168);
        }
    }
}
