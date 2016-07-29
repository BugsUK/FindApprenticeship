namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting
{
    using Apprenticeships.Application.VacancyPosting;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyPostingServiceTests : TestBase
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

        [SetUp]
        public void SetUp()
        {
            VacancyPostingService = new VacancyPostingService(
                MockApprenticeshipVacancyReadRepository.Object,
                MockApprenticeshipVacancyWriteRepository.Object,
                MockReferenceNumberRepository.Object,
                MockProviderUserReadRepository.Object,
                MockVacancyLocationAddressReadRepository.Object,
                MockVacancyLocationAddressWriteRepository.Object,
                MockCurrentUserService.Object,
                MockProviderVacancyAuthorisationService.Object);

            MockProviderUserReadRepository.Setup(r => r.GetByUsername(_vacancyManager.Username)).Returns(_vacancyManager);
            MockProviderUserReadRepository.Setup(r => r.GetByUsername(_lastEditedBy.Username)).Returns(_lastEditedBy);
        }

        [Test]
        public void CreateVacancyShouldCallRepository()
        {
            // Arrange.
            MockCurrentUserService.Setup(cus => cus.CurrentUserName).Returns(_vacancyManager.Username);
            MockCurrentUserService.Setup(cus => cus.IsInRole(Roles.Faa)).Returns(true);

            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1,
                VacancyId = 1
            };

            MockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            // Act.
            VacancyPostingService.CreateVacancy(vacancy);

            // Assert.
            MockApprenticeshipVacancyWriteRepository.Verify(r => r.Create(vacancy));
        }

        [Test]
        public void CreateVacancyShouldUpdateVacancyManagerAndDeliveryOrganisation()
        {
            // Arrange.
            MockCurrentUserService.Setup(cus => cus.IsInRole(Roles.Faa)).Returns(true);
            MockCurrentUserService.Setup(cus => cus.CurrentUserName).Returns(_vacancyManager.Username);

            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1,
                VacancyId = 1
            };

            MockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            // ReSharper disable once PossibleInvalidOperationException
            var preferredProviderSiteId = _vacancyManager.PreferredProviderSiteId.Value;

            // Act.
            VacancyPostingService.CreateVacancy(vacancy);

            // Assert.
            MockApprenticeshipVacancyWriteRepository.Verify(r => r.Create(It.Is<Vacancy>(v =>
                v.VacancyManagerId == preferredProviderSiteId &&
                v.DeliveryOrganisationId == preferredProviderSiteId)));
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

            MockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            // Act.
            VacancyPostingService.CreateVacancy(vacancy);

            // Assert.
            MockApprenticeshipVacancyWriteRepository.Verify(r => r.Create(It.Is<Vacancy>(v => v.VacancySource == VacancySource.Raa)));
        }

        [Test]
        public void GetNextVacancyReferenceNumberShouldCallRepository()
        {
            // Arrange.
            VacancyPostingService.GetNextVacancyReferenceNumber();

            MockReferenceNumberRepository.Verify(r => r.GetNextVacancyReferenceNumber());
        }

        [Test]
        public void GetVacancyByReferenceNumberShouldCallRepository()
        {
            // Arrange.
            const int vacancyReferenceNumber = 1;

            // Act.
            VacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            // Assert.
            MockApprenticeshipVacancyReadRepository.Verify(r => r.GetByReferenceNumber(vacancyReferenceNumber));
        }

        [Test]
        public void GetVacancyByGuidShouldCallRepository()
        {
            // Arrange.
            const int vacancyId = 42;

            // Act.
            VacancyPostingService.GetVacancy(vacancyId);

            // Assert.
            MockApprenticeshipVacancyReadRepository.Verify(r => r.Get(vacancyId));
        }
    }
}
