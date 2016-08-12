namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting.Strategies
{
    using System;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Providers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class UpdateVacancyStrategyTests
    {
        private readonly Vacancy _testVacancy = new Fixture().Create<Vacancy>();

        private readonly Mock<IVacancyReadRepository> _mockApprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
        private readonly Mock<IVacancyWriteRepository> _mockApprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();
        private readonly Mock<IProviderUserReadRepository> _mockProviderUserReadRepository = new Mock<IProviderUserReadRepository>();
        private readonly Mock<ICurrentUserService> _mockCurrentUserService = new Mock<ICurrentUserService>();
        private readonly Mock<IProviderVacancyAuthorisationService> _mockProviderVacancyAuthorisationService = new Mock<IProviderVacancyAuthorisationService>();

        private IUpdateVacancyStrategy _updateVacancyStrategy;
        private IUpsertVacancyStrategy _upsertVacancyStrategy;

        [SetUp]
        public void SetUp()
        {
            _mockProviderVacancyAuthorisationService.Setup(mock => mock.Authorise(_testVacancy)).Throws<UnauthorizedAccessException>();

            _upsertVacancyStrategy = new UpsertVacancyStrategy(_mockCurrentUserService.Object, _mockProviderUserReadRepository.Object, _mockApprenticeshipVacancyReadRepository.Object, _mockProviderVacancyAuthorisationService.Object);
            _updateVacancyStrategy = new UpdateVacancyStrategy(_mockApprenticeshipVacancyWriteRepository.Object, _upsertVacancyStrategy);
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnUpdateVacancy()
        {
            // Act.
            Action action = () => _updateVacancyStrategy.UpdateVacancy(_testVacancy);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }
    }
}