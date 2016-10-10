namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.VacancyOwnerRelationship
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Application.Interfaces;
    using Sql.Common;
    using Sql.Schemas.dbo;

    [TestFixture]
    public class VacancyOwnerRelationshipRepositoryTests
    {
        private readonly IMapper _mapper = new VacancyOwnerRelationshipMappers();
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();
        private IGetOpenConnection _connection;
        private IVacancyOwnerRelationshipReadRepository _vacancyOwnerRelationshipReadRepository;
        private IVacancyOwnerRelationshipWriteRepository _vacancyOwnerRelationshipWriteRepository;

        [SetUp]
        public void SetUp()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _vacancyOwnerRelationshipReadRepository = new VacancyOwnerRelationshipRepository(_connection, _mapper, _logger.Object);
            _vacancyOwnerRelationshipWriteRepository = new VacancyOwnerRelationshipRepository(_connection, _mapper, _logger.Object);
        }

        [Test]
        public void ShouldGetByProviderSiteAndEmployerId()
        {
            // Act.
            var providerSiteId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            var employerId = SeedData.Employers.AcmeCorp.EmployerId;

            var vacancyOwnerRelationshipByProviderSiteAndEmployerId = _vacancyOwnerRelationshipReadRepository.GetByProviderSiteAndEmployerId(providerSiteId, employerId);

            // Assert.
            vacancyOwnerRelationshipByProviderSiteAndEmployerId.Should().NotBeNull();

            // Act.
            var vacancyOwnerRelationshipById = _vacancyOwnerRelationshipReadRepository.GetByIds(new[] { vacancyOwnerRelationshipByProviderSiteAndEmployerId.VacancyOwnerRelationshipId }).FirstOrDefault();

            // Assert.
            vacancyOwnerRelationshipById.Should().NotBeNull();
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void ShouldGetByProviderSiteId()
        {
            // Act.
            var providerSiteId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            var vacancyParties = _vacancyOwnerRelationshipReadRepository.GetByProviderSiteId(providerSiteId);

            // Assert.
            vacancyParties.Should().NotBeNull();
            vacancyParties.Count().Should().BeGreaterThan(1);
        }

        [Test]
        public void ShouldSaveVacancyOwnerRelationship()
        {
            // Arrange.
            var newVacancyOwnerRelationship = new VacancyOwnerRelationship
            {
                VacancyOwnerRelationshipId = 0, // zero indicates new Vacancy Party
                ProviderSiteId = SeedData.ProviderSites.HopwoodHallCollege.ProviderSiteId,
                EmployerId = SeedData.Employers.AwesomeInc.EmployerId,
                EmployerDescription = "Some employer description.",
                EmployerWebsiteUrl = "http://example.com"
            };

            // Act.
            var savedVacancyOwnerRelationship = _vacancyOwnerRelationshipWriteRepository.Save(newVacancyOwnerRelationship);

            // Assert.
            savedVacancyOwnerRelationship.Should().NotBeNull();
            savedVacancyOwnerRelationship.VacancyOwnerRelationshipId.Should().NotBe(0);

            // Arrange.
            var newEmployerDescription = new string(savedVacancyOwnerRelationship.EmployerDescription.Reverse().ToArray());

            savedVacancyOwnerRelationship.EmployerDescription = newEmployerDescription;

            // Act.
            var updatedVacancyOwnerRelationship = _vacancyOwnerRelationshipWriteRepository.Save(savedVacancyOwnerRelationship);

            updatedVacancyOwnerRelationship.ShouldBeEquivalentTo(savedVacancyOwnerRelationship);
        }
    }
}
