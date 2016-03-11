namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.dbo.VacancyParty
{
    using System;
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
        public void ShouldGetVacancyPartyById()
        {
            // Act.
            var vacancyParty = _vacancyPartyReadRepository.GetById(1);

            // Assert.
            vacancyParty.Should().NotBeNull();
        }

        [Test]
        public void ShouldGetByProviderSiteAndEmployerId()
        {
            // Act.
            var vacancyParty = _vacancyPartyReadRepository.GetByProviderSiteAndEmployerId(1, 1);

            // Assert.
            vacancyParty.Should().NotBeNull();
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void ShouldGetByProviderSiteId()
        {
            // Act.
            var vacancyParties = _vacancyPartyReadRepository.GetByProviderSiteId(1);

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
                ProviderSiteId = 3,
                EmployerId = 1,
                EmployerDescription = "Some employer description.",
                EmployerWebsiteUrl = "http://example.com"
            };

            // Act.
            var savedVacancyParty = _vacancyPartyWriteRepository.Save(newVacancyParty);

            // Assert.
            savedVacancyParty.Should().NotBeNull();
            savedVacancyParty.VacancyPartyId.Should().BePositive();

            // Arrange.
            var newEmployerDescription = new string(savedVacancyParty.EmployerDescription.Reverse().ToArray());

            savedVacancyParty.EmployerDescription = newEmployerDescription;

            // Act.
            var updatedVacancyParty = _vacancyPartyWriteRepository.Save(savedVacancyParty);

            updatedVacancyParty.ShouldBeEquivalentTo(savedVacancyParty);
        }
    }
}
