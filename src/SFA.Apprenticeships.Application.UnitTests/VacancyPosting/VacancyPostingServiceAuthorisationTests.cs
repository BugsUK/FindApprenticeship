namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting
{
    using System;
    using Apprenticeships.Application.VacancyPosting;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class VacancyPostingServiceAuthorisationTests : TestBase
    {
        private readonly Vacancy _testVacancy = new Fixture().Create<Vacancy>();

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

            MockCurrentUserService.Setup(mock => mock
                .IsInRole(Roles.Faa))
                .Returns(true);

            MockProviderVacancyAuthorisationService.Setup(mock => mock
                .Authorise(_testVacancy.ProviderId, _testVacancy.VacancyManagerId))
                .Throws<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnCreateVacancy()
        {
            // Act.
            Action action = () => VacancyPostingService.CreateVacancy(_testVacancy);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnUpdateVacancy()
        {
            // Act.
            Action action = () => VacancyPostingService.UpdateVacancy(_testVacancy);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnGetVacancyByGuid()
        {
            MockApprenticeshipVacancyReadRepository.Setup(mock => mock
                .GetByVacancyGuid(_testVacancy.VacancyGuid))
                .Returns(_testVacancy);

            // Act.
            Action action = () => VacancyPostingService.GetVacancy(_testVacancy.VacancyGuid);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnGetVacancyById()
        {
            MockApprenticeshipVacancyReadRepository.Setup(mock => mock
                .Get(_testVacancy.VacancyId))
                .Returns(_testVacancy);

            // Act.
            Action action = () => VacancyPostingService.GetVacancy(_testVacancy.VacancyId);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldAuthoriseProviderUserOnGetVacancyByReferenceNumber()
        {
            MockApprenticeshipVacancyReadRepository.Setup(mock => mock
                .GetByReferenceNumber(_testVacancy.VacancyReferenceNumber))
                .Returns(_testVacancy);

            // Act.
            Action action = () => VacancyPostingService.GetVacancyByReferenceNumber(_testVacancy.VacancyReferenceNumber);

            // Assert.
            action.ShouldThrow<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldHandleVacancyNotFound()
        {
            MockApprenticeshipVacancyReadRepository.Setup(mock => mock
                .GetByReferenceNumber(_testVacancy.VacancyReferenceNumber))
                .Returns(default(Vacancy));

            // Act.
            Action action = () => VacancyPostingService.GetVacancyByReferenceNumber(_testVacancy.VacancyReferenceNumber);

            // Assert.
            action.ShouldNotThrow();
        }
    }
}
