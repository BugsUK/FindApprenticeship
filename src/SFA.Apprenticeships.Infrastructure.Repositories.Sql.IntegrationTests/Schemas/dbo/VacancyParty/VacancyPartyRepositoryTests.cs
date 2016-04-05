namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.VacancyParty
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.dbo;

    [TestFixture]
    public class VacancyPartyRepositoryTests
    {
        private readonly IMapper _mapper = new VacancyPartyMappers();
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();
        private IGetOpenConnection _connection;
        private IVacancyPartyReadRepository _vacancyPartyReadRepository;
        private IVacancyPartyWriteRepository _vacancyPartyWriteRepository;

        [SetUp]
        public void SetUp()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _vacancyPartyReadRepository = new VacancyPartyRepository(_connection, _mapper, _logger.Object);
            _vacancyPartyWriteRepository = new VacancyPartyRepository(_connection, _mapper, _logger.Object);
        }

        [Test]
        public void ShouldGetByProviderSiteAndEmployerId()
        {
            // Act.
            var providerSiteId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            var employerId = SeedData.Employers.AcmeCorp.EmployerId;

            var vacancyPartyByProviderSiteAndEmployerId = _vacancyPartyReadRepository.GetByProviderSiteAndEmployerId(providerSiteId, employerId);

            // Assert.
            vacancyPartyByProviderSiteAndEmployerId.Should().NotBeNull();

            // Act.
            var vacancyPartyById = _vacancyPartyReadRepository.GetById(vacancyPartyByProviderSiteAndEmployerId.VacancyPartyId);

            // Assert.
            vacancyPartyById.Should().NotBeNull();
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void ShouldGetByProviderSiteId()
        {
            // Act.
            var providerSiteId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            var vacancyParties = _vacancyPartyReadRepository.GetByProviderSiteId(providerSiteId);

            // Assert.
            vacancyParties.Should().NotBeNull();
            vacancyParties.Count().Should().BeGreaterThan(1);
        }

        [Test]
        public void ShouldSaveVacancyParty()
        {
            // Arrange.
            var newVacancyParty = new VacancyParty
            {
                VacancyPartyId = 0, // zero indicates new Vacancy Party
                ProviderSiteId = SeedData.ProviderSites.HopwoodHallCollege.ProviderSiteId,
                EmployerId = SeedData.Employers.AwesomeInc.EmployerId,
                EmployerDescription = "Some employer description.",
                EmployerWebsiteUrl = "http://example.com"
            };

            // Act.
            var savedVacancyParty = _vacancyPartyWriteRepository.Save(newVacancyParty);

            // Assert.
            savedVacancyParty.Should().NotBeNull();
            savedVacancyParty.VacancyPartyId.Should().NotBe(0);

            // Arrange.
            var newEmployerDescription = new string(savedVacancyParty.EmployerDescription.Reverse().ToArray());

            savedVacancyParty.EmployerDescription = newEmployerDescription;

            // Act.
            var updatedVacancyParty = _vacancyPartyWriteRepository.Save(savedVacancyParty);

            updatedVacancyParty.ShouldBeEquivalentTo(savedVacancyParty);
        }
    }
}
