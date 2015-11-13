namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting
{
    using System;
    using Application.VacancyPosting;
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
        private IVacancyPostingService _vacancyPostingService;

        [SetUp]
        public void SetUp()
        {
            _vacancyPostingService = new VacancyPostingService(_apprenticeshipVacancyReadRepository.Object,
                _apprenticeshipVacancyWriteRepository.Object, _referenceNumberRepository.Object);
        }

        [Test]
        public void SaveVacancyShouldCallRepository()
        {
            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 1
            };

            _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            _apprenticeshipVacancyWriteRepository.Verify(r => r.Save(vacancy));
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
            const long vacancyId = 1;

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
    }
}