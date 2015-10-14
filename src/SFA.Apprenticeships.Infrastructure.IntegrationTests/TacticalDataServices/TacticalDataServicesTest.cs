using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.TacticalDataServices
{
    using System.Linq;
    using Application.Organisation;
    using Application.ReferenceData;
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
            var categories = _referenceDataProvider.GetCategories();
            categories.Count().Should().BeGreaterThan(0);

            foreach (Category category in categories)
            {
                category.SubCategories.Count().Should().BeGreaterThan(0);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerSiteErn"></param>
        [TestCase("902763946"), Category("Integration")]
        public void ReturnsProviderSiteEmployerLinks(string providerSiteErn)
        {
            //Arrange
            var parameters = new EmployerSearchRequest(providerSiteErn);

            //Act
            var links = _legacyProviderProvider.GetProviderSiteEmployerLinks(parameters);

            //Assert
            links.Count().Should().Be(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerSiteErn"></param>
        /// <param name="ern"></param>
        /// <param name="employerName"></param>
        /// <param name="tradingName"></param>
        /// <param name="postCode"></param>
        [TestCase("902763946", "", "", "", ""), Category("Integration")]
        [TestCase("902763946", "", "", "", ""), Category("Integration")]
        public void ReturnsProviderSiteEmployerLinks(string providerSiteErn, string ern, string employerName, string tradingName, string postCode)
        {
            //Arrange
            var parameters = new EmployerSearchRequest(providerSiteErn, ern, employerName, tradingName, postCode);

            //Act
            var links = _legacyProviderProvider.GetProviderSiteEmployerLinks(parameters);

            //Assert
            links.Count().Should().BeGreaterThan(1);
        }

    }
}
