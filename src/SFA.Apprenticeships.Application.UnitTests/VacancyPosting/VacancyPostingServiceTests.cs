namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting
{
    using Apprenticeships.Application.VacancyPosting;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.VacancyPosting;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class VacancyPostingServiceTests
    {
        private readonly Mock<IVacancyReadRepository> _apprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
        private readonly Mock<IVacancyWriteRepository> _apprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();
        private readonly Mock<IReferenceNumberRepository> _referenceNumberRepository = new Mock<IReferenceNumberRepository>();
        private readonly Mock<IProviderUserReadRepository> _providerUserReadRepository = new Mock<IProviderUserReadRepository>();
        private readonly Mock<IVacancyLocationReadRepository> _vacancyLocationAddressReadRepository = new Mock<IVacancyLocationReadRepository>();
        private readonly Mock<IVacancyLocationWriteRepository> _vacancyLocationAddressWriteRepository = new Mock<IVacancyLocationWriteRepository>();
        private readonly Mock<ICurrentUserService> _currentUserService = new Mock<ICurrentUserService>();

        private IVacancyPostingService _vacancyPostingService;

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
            _vacancyPostingService = new VacancyPostingService(_apprenticeshipVacancyReadRepository.Object,
                _apprenticeshipVacancyWriteRepository.Object, _referenceNumberRepository.Object,
                _providerUserReadRepository.Object, _vacancyLocationAddressReadRepository.Object, 
                _vacancyLocationAddressWriteRepository.Object, _currentUserService.Object);

            _providerUserReadRepository.Setup(r => r.GetByUsername(_vacancyManager.Username)).Returns(_vacancyManager);
            _providerUserReadRepository.Setup(r => r.GetByUsername(_lastEditedBy.Username)).Returns(_lastEditedBy);
            }

        [Test]
        public void CreateVacancyShouldCallRepository()
        {
            _currentUserService.Setup(cus => cus.CurrentUserName).Returns(_vacancyManager.Username);
            _currentUserService.Setup(cus => cus.IsInRole(Roles.Faa)).Returns(true);

            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1,
                VacancyId = 1
            };

            _apprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.Create(vacancy));
        }

        [Test]
        public void CreateVacancyShouldUpdateVacancyManagerUsername()
        {
            _currentUserService.Setup(cus => cus.IsInRole(Roles.Faa)).Returns(true);
            _currentUserService.Setup(cus => cus.CurrentUserName).Returns(_vacancyManager.Username);

            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1,
                VacancyId = 1
            };

            _apprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.Create(It.Is<Vacancy>(v => v.VacancyManagerId == _vacancyManager.PreferredProviderSiteId.Value)));
        }

        [Test]
        public void SaveVacancyShouldCallRepository()
        {
            _currentUserService.Setup(cus => cus.IsInRole(Roles.Faa)).Returns(true);
            _currentUserService.Setup(cus => cus.CurrentUserName).Returns(_lastEditedBy.Username);

            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1,
                VacancyId = 1
            };

            _apprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            _vacancyPostingService.SaveVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.Create(vacancy));
        }

        [Test]
        public void SaveVacancyShouldUpdateLastEditedByUsername()
        {
            _currentUserService.Setup(cus => cus.IsInRole(Roles.Faa)).Returns(true);
            _currentUserService.Setup(cus => cus.CurrentUserName).Returns(_lastEditedBy.Username);

            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1,
                VacancyId = 1
            };
            _apprenticeshipVacancyWriteRepository.Setup(r => r.Create(vacancy)).Returns(vacancy);

            _vacancyPostingService.SaveVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.Create(It.Is<Vacancy>(v => v.LastEditedById == _lastEditedBy.ProviderUserId)));
        }

        [Test]
        public void GetNextVacancyReferenceNumberShouldCallRepository()
        {
            _vacancyPostingService.GetNextVacancyReferenceNumber();

            _referenceNumberRepository.Verify(r => r.GetNextVacancyReferenceNumber());
        }

        [Test]
        public void GetVacancyByReferenceNumberShouldCallRepository()
        {
            const int vacancyReferenceNumber = 1;

            _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            _apprenticeshipVacancyReadRepository.Verify(r => r.GetByReferenceNumber(vacancyReferenceNumber));
        }

        [Test]
        public void GetVacancyByGuidShouldCallRepository()
        {
            var vacancyId = 42;

            _vacancyPostingService.GetVacancy(vacancyId);

            _apprenticeshipVacancyReadRepository.Verify(r => r.Get(vacancyId));
        }
    }
}
