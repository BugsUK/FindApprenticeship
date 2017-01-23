namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.EmployerDataService
{
    using System.Linq;
    using Application.Interfaces;
    using Application.Interfaces.ReferenceData;
    using Application.Location.IoC;
    using Application.Organisation;
    using Application.ReferenceData;
    using Common.IoC;
    using FluentAssertions;
    using Infrastructure.EmployerDataService.IoC;
    using Infrastructure.Postcode.IoC;
    using Infrastructure.Raa;
    using Infrastructure.Raa.Strategies;
    using Logging.IoC;
    using NUnit.Framework;
    using Repositories.Sql.Configuration;
    using Repositories.Sql.IoC;
    using StructureMap;

    [TestFixture]
    public class EmployerDataServiceTests
    {
        private IVerifiedOrganisationProvider _verifiedOrganisationProvider;

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
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<EmployerDataServicesRegistry>();
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<LocationServiceRegistry>();
                x.AddRegistry(new RepositoriesRegistry(sqlConfiguration));

                x.For<IReferenceDataService>().Use<ReferenceDataService>();
                x.For<IReferenceDataProvider>().Use<ReferenceDataProvider>();
                x.For<IGetReleaseNotesStrategy>().Use<GetReleaseNotesStrategy>();
            });

            _verifiedOrganisationProvider = container.GetInstance<IVerifiedOrganisationProvider>();
        }

        [Test]
        [Category("Integration")]
        [Category("SmokeTests")]
        public void ShouldGetEmployerByReferenceNumberWithNoAliases()
        {
            // Arrange.
            const string referenceNumber = "130180483";

            // Act.
            var organisation = _verifiedOrganisationProvider.GetByReferenceNumber(referenceNumber);

            // Assert.
            organisation.Should().NotBeNull();
        }

        [Test]
        [Category("Integration")]
        [Category("SmokeTests")]
        public void ShouldGetEmployerByReferenceNumberWithAlias()
        {
            // Arrange.
            const string referenceNumber = "182211185";

            // Act.
            var organisation = _verifiedOrganisationProvider.GetByReferenceNumber(referenceNumber);

            // Assert.
            organisation.Should().NotBeNull();
            organisation.ReferenceNumber.Should().Be("182211185");
        }

        [TestCase("Babcock", null)]
        [TestCase("Babcock", "")]
        [TestCase("Starbucks", "CV1")]
        [Category("Integration")]
        [Category("SmokeTests")]
        public void ShouldFindEmployersByNameAndPostcode(string employerName, string postcode)
        {
            // Act.
            int resultsCount;
            var organisations = _verifiedOrganisationProvider.Find(employerName, postcode, out resultsCount);

            // Assert.
            organisations.Should().NotBeNull();
            organisations.Any().Should().BeTrue();
        }
    }
}
