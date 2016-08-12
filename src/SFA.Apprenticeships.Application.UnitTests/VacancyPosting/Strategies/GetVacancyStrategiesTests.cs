namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting.Strategies
{
    using System;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Providers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetVacancyStrategiesTests
    {
        private readonly Vacancy _testVacancy = new Fixture().Create<Vacancy>();

        private readonly Mock<IVacancyReadRepository> _mockApprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
        private readonly Mock<IProviderVacancyAuthorisationService> _mockProviderVacancyAuthorisationService = new Mock<IProviderVacancyAuthorisationService>();

        private IGetVacancyStrategies _getVacancyStrategies;
        private IAuthoriseCurrentUserStrategy _authoriseCurrentUserStrategy;

        [SetUp]
        public void SetUp()
        {
            _mockProviderVacancyAuthorisationService.Setup(mock => mock.Authorise(_testVacancy)).Throws<UnauthorizedAccessException>();

            _authoriseCurrentUserStrategy = new AuthoriseCurrentUserStrategy(_mockProviderVacancyAuthorisationService.Object);
            _getVacancyStrategies = new GetVacancyStrategies(_mockApprenticeshipVacancyReadRepository.Object, _authoriseCurrentUserStrategy);
        }

        [Test]
        public void GetVacancyByReferenceNumberShouldCallRepository()
        {
            // Arrange.
            const int vacancyReferenceNumber = 1;

            // Act.
            _getVacancyStrategies.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            // Assert.
            _mockApprenticeshipVacancyReadRepository.Verify(r => r.GetByReferenceNumber(vacancyReferenceNumber));
        }

        [Test]
        public void GetVacancyByGuidShouldCallRepository()
        {
            // Arrange.
            const int vacancyId = 42;

            // Act.
            _getVacancyStrategies.GetVacancyById(vacancyId);

            // Assert.
            _mockApprenticeshipVacancyReadRepository.Verify(r => r.Get(vacancyId));
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnGetVacancyByGuid()
        {
            _mockApprenticeshipVacancyReadRepository.Setup(mock => mock
                .GetByVacancyGuid(_testVacancy.VacancyGuid))
                .Returns(_testVacancy);

            // Act.
            Action action = () => _getVacancyStrategies.GetVacancyByGuid(_testVacancy.VacancyGuid);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnGetVacancyById()
        {
            _mockApprenticeshipVacancyReadRepository.Setup(mock => mock
                .Get(_testVacancy.VacancyId))
                .Returns(_testVacancy);

            // Act.
            Action action = () => _getVacancyStrategies.GetVacancyById(_testVacancy.VacancyId);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnGetVacancyByReferenceNumber()
        {
            _mockApprenticeshipVacancyReadRepository.Setup(mock => mock
                .GetByReferenceNumber(_testVacancy.VacancyReferenceNumber))
                .Returns(_testVacancy);

            // Act.
            Action action = () => _getVacancyStrategies.GetVacancyByReferenceNumber(_testVacancy.VacancyReferenceNumber);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldHandleVacancyNotFound()
        {
            _mockApprenticeshipVacancyReadRepository.Setup(mock => mock
                .GetByReferenceNumber(_testVacancy.VacancyReferenceNumber))
                .Returns(default(Vacancy));

            // Act.
            Action action = () => _getVacancyStrategies.GetVacancyByReferenceNumber(_testVacancy.VacancyReferenceNumber);

            // Assert.
            action.ShouldNotThrow();
        }
    }
}