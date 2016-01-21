namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.EmployerDataService
{
    using System.Linq;
    using Application.Organisation;
    using Common.IoC;
    using FluentAssertions;
    using Infrastructure.EmployerDataService.IoC;
    using Infrastructure.Postcode.IoC;
    using Logging.IoC;
    using NUnit.Framework;
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
                x.AddRegistry<EmployerDataServicesRegistry>();
                x.AddRegistry<PostcodeRegistry>();
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
            organisation.ReferenceNumber.Should().Be("193166879");
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
            organisations.ToArray().Count().Should().BePositive();
        }
    }
}
