using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.TacticalDataServices
{
    using System.Linq;
    using Application.Organisation;
    using Application.ReferenceData;
    using Common.Configuration;
    using Common.IoC;
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
        
        [TestCase("902763946"), Category("Integration")]
        public void ReturnsProviderSiteEmployerLinks(string providerSiteErn)
        {
            //Arrange
            var query = new EmployerSearchRequest(providerSiteErn);

            //Act
            var links = _legacyProviderProvider.GetProviderSiteEmployerLinks(query);

            //Assert
            links.Count().Should().Be(168);
        }

        [TestCase("902763946", "Hit Training", "LE3 1HR", 1), Category("Integration")]
        [TestCase("902763946", "", "LE3 1HR", 1), Category("Integration")]
        [TestCase("902763946", "Hit Training", "L E3 ", 1), Category("Integration")]
        [TestCase("902763946", "Hit Training", "LE3 1HR", 1), Category("Integration")]
        [TestCase("902763946", "Hit Training", "LE3 1HRz", 0), Category("Integration")]
        [TestCase("902763946", "Hit Training", "", 1), Category("Integration")]
        public void ReturnsProviderSiteEmployerLinksByNameAndPostcode(string providerSiteErn, string employerName, string postCode, int expectedResults)
        {
            //Arrange
            var query = new EmployerSearchRequest(providerSiteErn, employerName, postCode);

            //Act
            var links = _legacyProviderProvider.GetProviderSiteEmployerLinks(query);

            //Assert
            links.Count().Should().Be(expectedResults);
        }

        [TestCase("902763946", "902763946"), Category("Integration")]
        [TestCase("902763946", "105108332"), Category("Integration")]
        [TestCase("902763946", "162258372"), Category("Integration")]
        public void SearchProviderSiteEmployerLinksByEmployerEdsUrn(string providerSiteErn, string employerEdsUrn)
        {
            //Arrange
            var query= new EmployerSearchRequest(providerSiteErn, employerEdsUrn);

            //Act
            var links = _legacyProviderProvider.GetProviderSiteEmployerLinks(query);

            //Assert
            //there should only ever be 1, as employer edsurn is unique
            links.Count().Should().Be(1);
        }

    }
}
