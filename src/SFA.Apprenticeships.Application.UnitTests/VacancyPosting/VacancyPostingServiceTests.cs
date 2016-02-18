namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Application.VacancyPosting;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.VacancyPosting;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyPostingServiceTests
    {
        private readonly Mock<IVacancyReadRepository> _apprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
        private readonly Mock<IVacancyWriteRepository> _apprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();
        private readonly Mock<IReferenceNumberRepository> _referenceNumberRepository = new Mock<IReferenceNumberRepository>();
        private readonly Mock<IProviderUserReadRepository> _providerUserReadRepository = new Mock<IProviderUserReadRepository>();
        private readonly Mock<IVacancyLocationAddressReadRepository> _vacancyLocationAddressReadRepository = new Mock<IVacancyLocationAddressReadRepository>();
        private IVacancyPostingService _vacancyPostingService;

        private readonly ProviderUser _vacancyManager = new ProviderUser
        {
            ProviderUserId = 1,
            Username = "vacancy@manager.com"
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
                _providerUserReadRepository.Object, _vacancyLocationAddressReadRepository.Object);

            _providerUserReadRepository.Setup(r => r.Get(_vacancyManager.Username)).Returns(_vacancyManager);
            _providerUserReadRepository.Setup(r => r.Get(_lastEditedBy.Username)).Returns(_lastEditedBy);
        }

        [Test]
        public void CreateVacancyShouldCallRepository()
        {
            var principal = new ClaimsPrincipalBuilder().WithName(_vacancyManager.Username).WithRole(Roles.Faa).Build();
            Thread.CurrentPrincipal = principal;
            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1
            };

            _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.Save(vacancy));
        }

        [Test]
        public void CreateVacancyShouldUpdateVacancyManagerUsername()
        {
            var principal = new ClaimsPrincipalBuilder().WithName(_vacancyManager.Username).WithRole(Roles.Faa).Build();
            Thread.CurrentPrincipal = principal;
            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1
            };

            _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.Save(It.Is<Vacancy>(v => v.VacancyManagerId == _vacancyManager.ProviderUserId)));
        }

        [Test]
        public void SaveVacancyShouldCallRepository()
        {
            var principal = new ClaimsPrincipalBuilder().WithName(_lastEditedBy.Username).WithRole(Roles.Faa).Build();
            Thread.CurrentPrincipal = principal;
            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1
            };

            _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.Save(vacancy));
        }

        [Test]
        public void SaveVacancyShouldUpdateLastEditedByUsername()
        {
            var principal = new ClaimsPrincipalBuilder().WithName(_lastEditedBy.Username).WithRole(Roles.Faa).Build();
            Thread.CurrentPrincipal = principal;
            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = 1
            };

            _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.Save(It.Is<Vacancy>(v => v.LastEditedById == _lastEditedBy.ProviderUserId)));
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
            const long vacancyReferenceNumber = 1;

            _vacancyPostingService.GetVacancy(vacancyReferenceNumber);

            _apprenticeshipVacancyReadRepository.Verify(r => r.GetByReferenceNumber(vacancyReferenceNumber));
        }

        [Test]
        public void GetVacancyByGuidShouldCallRepository()
        {
            var vacancyId = 42;

            _vacancyPostingService.GetVacancy(vacancyId);

            _apprenticeshipVacancyReadRepository.Verify(r => r.Get(vacancyId));
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
            const long vacancyReferenceNumber = 1L;

            _vacancyPostingService.ReplaceLocationInformation(vacancyReferenceNumber, isEmployerLocationMainApprenticeshipLocation,
                numberOfPositions, vacancyLocationAddresses, locationAddressesComment, additionalLocationInformation,
                additionalLocationInformationComment);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.ReplaceLocationInformation(vacancyReferenceNumber, isEmployerLocationMainApprenticeshipLocation,
                numberOfPositions, vacancyLocationAddresses, locationAddressesComment, additionalLocationInformation,
                additionalLocationInformationComment));
        }
    }
}
