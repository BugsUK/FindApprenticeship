namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting.Strategies
{
    using System;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces;
    using Interfaces.Providers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class CreateVacancyStrategyTests
    {
        private readonly ProviderUser _vacancyManager = new ProviderUser
        {
            ProviderUserId = 1,
            Username = "vacancy@manager.com",
            PreferredProviderSiteId = 10
        };

        private readonly ProviderUser _lastEditedBy = new ProviderUser
        {
            ProviderUserId = 2,
            Username = "vacancy@editor.com"
        };

        private readonly Vacancy _testVacancy = new Fixture().Create<Vacancy>();

        private readonly Mock<IVacancyReadRepository> _mockApprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
        private readonly Mock<IVacancyWriteRepository> _mockApprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();
        private readonly Mock<IProviderUserReadRepository> _mockProviderUserReadRepository = new Mock<IProviderUserReadRepository>();
        private readonly Mock<ICurrentUserService> _mockCurrentUserService = new Mock<ICurrentUserService>();
        private readonly Mock<IProviderVacancyAuthorisationService> _mockProviderVacancyAuthorisationService = new Mock<IProviderVacancyAuthorisationService>();
        private readonly Mock<IProviderService> _mockProviderService = new Mock<IProviderService>();

        private ICreateVacancyStrategy _createVacancyStrategy;

        [SetUp]
        public void SetUp()
        {
            _mockProviderUserReadRepository.Setup(r => r.GetByUsername(_vacancyManager.Username)).Returns(_vacancyManager);
            _mockProviderUserReadRepository.Setup(r => r.GetByUsername(_lastEditedBy.Username)).Returns(_lastEditedBy);
            _mockProviderVacancyAuthorisationService.Setup(mock => mock.Authorise(_testVacancy)).Throws<UnauthorizedAccessException>();
            _mockProviderService.Setup(ps => ps.GetVacancyParty(It.IsAny<int>(), false)).Returns(new Fixture().Create<VacancyParty>());

            var upsertVacancyStrategy = new UpsertVacancyStrategy(_mockCurrentUserService.Object, _mockProviderUserReadRepository.Object, _mockApprenticeshipVacancyReadRepository.Object, new AuthoriseCurrentUserStrategy(_mockProviderVacancyAuthorisationService.Object), new Mock<IPublishVacancySummaryUpdateStrategy>().Object);
            _createVacancyStrategy = new CreateVacancyStrategy(_mockApprenticeshipVacancyWriteRepository.Object, upsertVacancyStrategy, _mockProviderService.Object);
        }

        [Test]
        public void CreateVacancyShouldCallRepository()
        {
            // Arrange.
            _mockCurrentUserService.Setup(cus => cus.CurrentUserName).Returns(_vacancyManager.Username);
            _mockCurrentUserService.Setup(cus => cus.IsInRole(Roles.Faa)).Returns(true);

            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1,
                VacancyId = 1
            };

            _mockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            // Act.
            _createVacancyStrategy.CreateVacancy(vacancy);

            // Assert.
            _mockApprenticeshipVacancyWriteRepository.Verify(r => r.Create(vacancy));
        }

        [Test]
        public void CreateVacancyShouldUpdateVacancyManagerAndDeliveryOrganisation()
        {
            // Arrange.
            const int providerSiteId = 12345;
            _mockCurrentUserService.Setup(cus => cus.IsInRole(Roles.Faa)).Returns(true);
            _mockCurrentUserService.Setup(cus => cus.CurrentUserName).Returns(_vacancyManager.Username);
            _mockProviderService.Setup(ps => ps.GetVacancyParty(It.IsAny<int>(), false)).Returns(new Fixture().Build<VacancyParty>().With(vor => vor.ProviderSiteId, providerSiteId).Create());

            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1,
                VacancyId = 1
            };

            _mockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            // Act.
            _createVacancyStrategy.CreateVacancy(vacancy);

            // Assert.
            _mockApprenticeshipVacancyWriteRepository.Verify(r => r.Create(It.Is<Vacancy>(v =>
                v.VacancyManagerId == providerSiteId &&
                v.DeliveryOrganisationId == providerSiteId)));
        }

        [Test]
        public void CreateVacancyShouldSetVacancySourceAsRaa()
        {
            // Arrange.
            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1,
                VacancyId = 1
            };

            _mockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            // Act.
            _createVacancyStrategy.CreateVacancy(vacancy);

            // Assert.
            _mockApprenticeshipVacancyWriteRepository.Verify(r => r.Create(It.Is<Vacancy>(v => v.VacancySource == VacancySource.Raa)));
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnCreateVacancy()
        {
            // Act.
            Action action = () => _createVacancyStrategy.CreateVacancy(_testVacancy);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }
    }
}