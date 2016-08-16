namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;

    [TestFixture]
    [Parallelizable]
    public class ManageDatesTests : TestBase
    {
        [Test]
        public void ShouldUpdateClosingDate()
        {
            const int vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(closingDate),
                PossibleStartDate = new DateViewModel(possibleStartDate),
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new Vacancy { VacancyReferenceNumber = vacancyReferenceNumber};
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, VacancyDatesViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancy(viewModel);

            MockVacancyPostingService.Verify(s => s.UpdateVacancy(It.Is<Vacancy>(v => v.ClosingDate == closingDate)));
        }

        [Test]
        public void ShouldUpdatePossibleStartDate()
        {
            const int vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(closingDate),
                PossibleStartDate = new DateViewModel(possibleStartDate),
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new Vacancy { VacancyReferenceNumber = vacancyReferenceNumber };
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, VacancyDatesViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancy(viewModel);

            MockVacancyPostingService.Verify(s => s.UpdateVacancy(It.Is<Vacancy>(v => v.PossibleStartDate == possibleStartDate)));
        }

        [Test]
        public void ShouldUpdateStatusToLive()
        {
            const int vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(closingDate),
                PossibleStartDate = new DateViewModel(possibleStartDate),
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new Vacancy { VacancyReferenceNumber = vacancyReferenceNumber };
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, VacancyDatesViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancy(viewModel);

            MockVacancyPostingService.Verify(s => s.UpdateVacancy(It.Is<Vacancy>(v => v.Status == VacancyStatus.Live)));
        }

        [TestCase(0, VacancyApplicationsState.NoApplications)]
        [TestCase(1, VacancyApplicationsState.HasApplications)]
        public void ShouldSetVacancyApplicationStateAfterUpdate(int applicationCount, VacancyApplicationsState expectedState)
        {
            // Arrange.
            const int vacancyId = 1;
            const int vacancyReferenceNumber = 2;

            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new VacancyDatesViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                ClosingDate = new DateViewModel(closingDate),
                PossibleStartDate = new DateViewModel(possibleStartDate)
            };

            var vacancy = new Vacancy
            {
                VacancyId = vacancyId,
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            MockVacancyPostingService.Setup(mock => mock
                .GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(vacancy);

            MockVacancyPostingService.Setup(mock => mock
                .UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(vacancy);

            MockMapper.Setup(m => m.Map<Vacancy, VacancyDatesViewModel>(vacancy))
                .Returns(viewModel);

            MockApprenticeshipApplicationService.Setup(mock => mock.
                GetApplicationCount(vacancyId))
                .Returns(applicationCount);

            var provider = GetVacancyPostingProvider();

            // Act.
            var result = provider.UpdateVacancy(viewModel);

            // Assert.
            result.VacancyApplicationsState.Should().Be(expectedState);
        }

        [TestCase(0, VacancyApplicationsState.NoApplications)]
        [TestCase(1, VacancyApplicationsState.HasApplications)]
        public void ShouldGetVacancyApplicationState(int applicationCount, VacancyApplicationsState expectedState)
        {
            // Arrange.
            const int vacancyId = 1;
            const int vacancyReferenceNumber = 2;

            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new VacancyDatesViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                ClosingDate = new DateViewModel(closingDate),
                PossibleStartDate = new DateViewModel(possibleStartDate)
            };

            var vacancy = new Vacancy
            {
                VacancyId = vacancyId,
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            MockVacancyPostingService.Setup(mock => mock
                .GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(vacancy);

            MockMapper.Setup(m => m.Map<Vacancy, VacancyDatesViewModel>(vacancy))
                .Returns(viewModel);

            MockApprenticeshipApplicationService.Setup(mock => mock.
                GetApplicationCount(vacancyId))
                .Returns(applicationCount);

            var provider = GetVacancyPostingProvider();

            // Act.
            var result = provider.GetVacancyDatesViewModel(vacancyReferenceNumber);

            // Assert.
            result.VacancyApplicationsState.Should().Be(expectedState);
        }
    }
}