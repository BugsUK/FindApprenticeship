namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Application.VacancyPosting;
    using Domain.Entities;
    using Domain.Entities.Locations;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Repositories;
    using Interfaces.VacancyPosting;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyPostingServiceTests
    {
        private readonly Mock<IApprenticeshipVacancyReadRepository> _apprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
        private readonly Mock<IApprenticeshipVacancyWriteRepository> _apprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();
        private readonly Mock<IReferenceNumberRepository> _referenceNumberRepository = new Mock<IReferenceNumberRepository>();
        private readonly Mock<IProviderUserReadRepository> _providerUserReadRepository = new Mock<IProviderUserReadRepository>();
        private IVacancyPostingService _vacancyPostingService;

        private readonly ProviderUser _vacancyManager = new ProviderUser
        {
            ProviderUserId = 5,
            Username = "vacancy@manager.com"
        };

        private readonly ProviderUser _lastEditedBy = new ProviderUser
        {
            ProviderUserId = 42,
            Username = "vacancy@editor.com"
        };

        [SetUp]
        public void SetUp()
        {
            _vacancyPostingService = new VacancyPostingService(_apprenticeshipVacancyReadRepository.Object,
                _apprenticeshipVacancyWriteRepository.Object, _referenceNumberRepository.Object,
                _providerUserReadRepository.Object);

            _providerUserReadRepository.Setup(r => r.Get(_vacancyManager.Username)).Returns(_vacancyManager);
            _providerUserReadRepository.Setup(r => r.Get(_lastEditedBy.Username)).Returns(_lastEditedBy);
        }

        [Test]
        public void CreateVacancyShouldCallRepository()
        {
            var principal = new ClaimsPrincipalBuilder().WithName(_vacancyManager.Username).WithRole(Roles.Faa).Build();
            Thread.CurrentPrincipal = principal;
            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 1
            };

            _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.DeepSave(vacancy));
        }

        [Test]
        public void CreateVacancyShouldUpdateVacancyManagerUsername()
        {
            var principal = new ClaimsPrincipalBuilder().WithName(_vacancyManager.Username).WithRole(Roles.Faa).Build();
            Thread.CurrentPrincipal = principal;
            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 1
            };

            _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.DeepSave(It.Is<ApprenticeshipVacancy>(v => v.VacancyManagerId == _vacancyManager.ProviderUserId)));
        }

        [Test]
        public void SaveVacancyShouldCallRepository()
        {
            var principal = new ClaimsPrincipalBuilder().WithName(_lastEditedBy.Username).WithRole(Roles.Faa).Build();
            Thread.CurrentPrincipal = principal;
            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 1
            };

            _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.DeepSave(vacancy));
        }

        [Test]
        public void SaveVacancyShouldUpdateLastEditedByUsername()
        {
            var principal = new ClaimsPrincipalBuilder().WithName(_lastEditedBy.Username).WithRole(Roles.Faa).Build();
            Thread.CurrentPrincipal = principal;
            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 1
            };

            _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.DeepSave(It.Is<ApprenticeshipVacancy>(v => v.LastEditedById == _lastEditedBy.ProviderUserId)));
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
            const int vacancyId = 1;

            _vacancyPostingService.GetVacancy(vacancyId);

            _apprenticeshipVacancyReadRepository.Verify(r => r.Get(vacancyId));
        }

        [Test]
        public void GetVacancyByGuidShouldCallRepository()
        {
            var vacancyGuid = Guid.NewGuid();

            _vacancyPostingService.GetVacancy(vacancyGuid);

            _apprenticeshipVacancyReadRepository.Verify(r => r.Get(vacancyGuid));
        }

        [Test]
        public void ReplaceLocationInformationShouldCallRepository()
        {
            const bool isEmployerLocationMainApprenticeshipLocation = false;
            int? numberOfPositions = null;

            IEnumerable<VacancyLocationAddress> vacancyLocationAddresses = new []{new VacancyLocationAddress(), new VacancyLocationAddress(), new VacancyLocationAddress()};

            const string locationAddressesComment = "location addresses comment";
            const string additionalLocationInformation = "additional location information";
            const string additionalLocationInformationComment = "additional location information";
            var vacancyGuid = Guid.NewGuid();

            _vacancyPostingService.ReplaceLocationInformation(vacancyGuid, isEmployerLocationMainApprenticeshipLocation,
                numberOfPositions, vacancyLocationAddresses, locationAddressesComment, additionalLocationInformation,
                additionalLocationInformationComment);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.ReplaceLocationInformation(vacancyGuid, isEmployerLocationMainApprenticeshipLocation,
                numberOfPositions, vacancyLocationAddresses, locationAddressesComment, additionalLocationInformation,
                additionalLocationInformationComment));
        }
    }
}
