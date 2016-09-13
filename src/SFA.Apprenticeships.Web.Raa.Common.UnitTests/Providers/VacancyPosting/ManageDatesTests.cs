namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
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

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(),
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(closingDate),
                    PossibleStartDate = new DateViewModel(possibleStartDate)
                },
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new Vacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                Wage = new Wage(WageType.NationalMinimum, null, null, WageUnit.Weekly, 30)
            };
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, FurtherVacancyDetailsViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancyDates(viewModel);

            MockVacancyPostingService.Verify(s => s.UpdateVacancy(It.Is<Vacancy>(v => v.ClosingDate == closingDate)));
        }

        [Test]
        public void ShouldUpdatePossibleStartDate()
        {
            const int vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(),
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(closingDate),
                    PossibleStartDate = new DateViewModel(possibleStartDate)
                },
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new Vacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                Wage = new Wage(WageType.NationalMinimum, null, null, WageUnit.Weekly, 30)
            };
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, FurtherVacancyDetailsViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancyDates(viewModel);

            MockVacancyPostingService.Verify(s => s.UpdateVacancy(It.Is<Vacancy>(v => v.PossibleStartDate == possibleStartDate)));
        }

        [Test]
        public void ShouldUpdateWage()
        {
            const int vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(WageType.Custom, 450, null, WageUnit.Monthly, 37.5m),
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(closingDate),
                    PossibleStartDate = new DateViewModel(possibleStartDate)
                },
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new Vacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                Wage = new Wage(WageType.NationalMinimum, null, "Legacy text", WageUnit.Weekly, 30)
            };
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, FurtherVacancyDetailsViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancyDates(viewModel);

            MockVacancyPostingService.Verify(s => s.UpdateVacancy(It.Is<Vacancy>(
                v => v.PossibleStartDate == possibleStartDate
                && v.Wage.Type == viewModel.Wage.Type
                && v.Wage.Amount == viewModel.Wage.Amount
                && v.Wage.Text == apprenticeshipVacancy.Wage.Text
                && v.Wage.Unit == viewModel.Wage.Unit
                && v.Wage.HoursPerWeek == apprenticeshipVacancy.Wage.HoursPerWeek)));
        }

        [Test]
        public void ShouldUpdateStatusToLive()
        {
            const int vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(),
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(closingDate),
                    PossibleStartDate = new DateViewModel(possibleStartDate)
                },
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new Vacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                Wage = new Wage(WageType.NationalMinimum, null, null, WageUnit.Weekly, 30)
            };
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, FurtherVacancyDetailsViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancyDates(viewModel);

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

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(),
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(closingDate),
                    PossibleStartDate = new DateViewModel(possibleStartDate)
                },
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var vacancy = new Vacancy
            {
                VacancyId = vacancyId,
                VacancyReferenceNumber = vacancyReferenceNumber,
                Wage = new Wage(WageType.NationalMinimum, null, null, WageUnit.Weekly, 30)
            };

            MockVacancyPostingService.Setup(mock => mock
                .GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(vacancy);

            MockVacancyPostingService.Setup(mock => mock
                .UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(vacancy);

            MockMapper.Setup(m => m.Map<Vacancy, FurtherVacancyDetailsViewModel>(vacancy))
                .Returns(viewModel);

            MockApprenticeshipApplicationService.Setup(mock => mock.
                GetApplicationCount(vacancyId))
                .Returns(applicationCount);

            var provider = GetVacancyPostingProvider();

            // Act.
            var result = provider.UpdateVacancyDates(viewModel);

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

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(),
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(closingDate),
                    PossibleStartDate = new DateViewModel(possibleStartDate)
                },
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var vacancy = new Vacancy
            {
                VacancyId = vacancyId,
                VacancyReferenceNumber = vacancyReferenceNumber,
                Wage = new Wage(WageType.NationalMinimum, null, null, WageUnit.Weekly, 30)
            };

            MockVacancyPostingService.Setup(mock => mock
                .GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(vacancy);

            MockMapper.Setup(m => m.Map<Vacancy, FurtherVacancyDetailsViewModel>(vacancy))
                .Returns(viewModel);

            MockApprenticeshipApplicationService.Setup(mock => mock.
                GetApplicationCount(vacancyId))
                .Returns(applicationCount);

            var provider = GetVacancyPostingProvider();

            // Act.
            var result = provider.GetVacancySummaryViewModel(vacancyReferenceNumber);

            // Assert.
            result.VacancyApplicationsState.Should().Be(expectedState);
        }
    }
}