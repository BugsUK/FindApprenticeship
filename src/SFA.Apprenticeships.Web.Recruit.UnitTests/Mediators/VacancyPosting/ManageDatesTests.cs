namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using Builders;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    [Parallelizable]
    public class ManageDatesTests : TestsBase
    {
        [Test]
        public void ShouldReturnAWarningHashIfTheModelHasOnlyWarnings()
        {
            const int vacancyReferenceNumber = 1;

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(),
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.Now.AddDays(7)),
                    PossibleStartDate = new DateViewModel(DateTime.Now.AddDays(7))
                }
            };

            VacancyPostingProvider.Setup(p => p.GetVacancySummaryViewModel(vacancyReferenceNumber)).Returns(viewModel);
            var mediator = GetMediator();

            var result = mediator.GetVacancySummaryViewModel(vacancyReferenceNumber, true, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.GetVacancySummaryViewModel.FailedValidation);
            result.ViewModel.WarningsHash.Should().NotBe(0);
            result.ViewModel.Should().Be(viewModel);
        }

        [Test]
        public void ShouldReturnOkIfThereIsntAnyValidationError()
        {
            var vacancyReferenceNumber = 1;

            var viewModel = new VacancyViewModelBuilder().With(new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Now.AddDays(20)),
                PossibleStartDate = new DateViewModel(DateTime.Now.AddDays(21))
            }).BuildValid(VacancyStatus.Live, VacancyType.Apprenticeship).FurtherVacancyDetailsViewModel;

            VacancyPostingProvider.Setup(p => p.GetVacancySummaryViewModel(vacancyReferenceNumber)).Returns(viewModel);
            var mediator = GetMediator();

            var result = mediator.GetVacancySummaryViewModel(vacancyReferenceNumber, true, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.GetVacancySummaryViewModel.Ok);
            result.ViewModel.Should().Be(viewModel);
        }

        [Test]
        public void ShouldNotUpdateTheVacancyIfThereAreWarningsAndWeDontAcceptThem()
        {
            var vacancyReferenceNumber = 1;

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(),
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.Now.AddDays(7)),
                    PossibleStartDate = new DateViewModel(DateTime.Now.AddDays(7)),
                },
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var mediator = GetMediator();

            var result = mediator.UpdateVacancyDates(viewModel, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.ManageDates.FailedValidation);
        }

        [Test]
        public void ShouldNotUpdateTheVacancyIfWeAcceptTheWarningsButTheyAreDifferentFromThePreviousOnes()
        {
            const int vacancyReferenceNumber = 1;
            const int oldWarningHash = -1011218820;

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(),
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.Now.AddDays(20)),
                    PossibleStartDate = new DateViewModel(DateTime.Now.AddDays(20))
                },
                VacancyReferenceNumber = vacancyReferenceNumber,
                WarningsHash = oldWarningHash
            };

            var mediator = GetMediator();

            var result = mediator.UpdateVacancyDates(viewModel, true);

            result.Code.Should().Be(VacancyPostingMediatorCodes.ManageDates.FailedValidation);
        }

        [TestCase(VacancyApplicationsState.HasApplications, VacancyPostingMediatorCodes.ManageDates.UpdatedHasApplications)]
        [TestCase(VacancyApplicationsState.NoApplications, VacancyPostingMediatorCodes.ManageDates.UpdatedNoApplications)]
        public void ShouldUpdateTheVacancyIfWeAcceptTheWarningsAndTheyAreEqualFromThePreviousOnes(VacancyApplicationsState state, string expectedCode)
        {
            const int vacancyReferenceNumber = 1;
            const int oldWarningHash = 128335101;

            var closingDate = new DateViewModel(DateTime.Now.AddDays(20));
            var possibleStartDate = new DateViewModel(DateTime.Now.AddDays(21));

            var viewModel = new VacancyViewModelBuilder().With(new VacancyDatesViewModel
                {
                    ClosingDate = closingDate,
                    PossibleStartDate = possibleStartDate
                }).BuildValid(VacancyStatus.Live, VacancyType.Apprenticeship).FurtherVacancyDetailsViewModel;

            viewModel.VacancyReferenceNumber = vacancyReferenceNumber;
            viewModel.WarningsHash = oldWarningHash;
            viewModel.VacancyApplicationsState = state;

            var mediator = GetMediator();

            VacancyPostingProvider.Setup(p => p.UpdateVacancyDates(It.IsAny<FurtherVacancyDetailsViewModel>())).Returns(viewModel);

            var result = mediator.UpdateVacancyDates(viewModel, true);

            result.Code.Should().Be(expectedCode);
            VacancyPostingProvider.Verify(
                p =>
                    p.UpdateVacancyDates(
                        It.Is<FurtherVacancyDetailsViewModel>(
                            v =>
                                v.VacancyReferenceNumber == vacancyReferenceNumber 
                                && v.VacancyDatesViewModel.ClosingDate == closingDate 
                                && v.VacancyDatesViewModel.PossibleStartDate == possibleStartDate)));
        }
    }
}