namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using Common.ViewModels;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class ManageDatesTests : TestsBase
    {
        [Test]
        public void ShouldReturnAWarningHashIfTheModelHasOnlyWarnings()
        {
            const int vacancyReferenceNumber = 1;

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Now.AddDays(7)),
                PossibleStartDate = new DateViewModel(DateTime.Now.AddDays(7))
            };

            VacancyPostingProvider.Setup(p => p.GetVacancyDatesViewModel(vacancyReferenceNumber)).Returns(viewModel);
            var mediator = GetMediator();

            var result = mediator.GetVacancyDatesViewModel(vacancyReferenceNumber);

            result.Code.Should().Be(VacancyPostingMediatorCodes.ManageDates.FailedValidation);
            result.ViewModel.WarningsHash.Should().NotBe(0);
            result.ViewModel.Should().Be(viewModel);
        }

        [Test]
        public void ShouldReturnOkIfThereIsntAnyValidationError()
        {
            var vacancyReferenceNumber = 1;

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Now.AddDays(20)),
                PossibleStartDate = new DateViewModel(DateTime.Now.AddDays(21))
            };

            VacancyPostingProvider.Setup(p => p.GetVacancyDatesViewModel(vacancyReferenceNumber)).Returns(viewModel);
            var mediator = GetMediator();

            var result = mediator.GetVacancyDatesViewModel(vacancyReferenceNumber);

            result.Code.Should().Be(VacancyPostingMediatorCodes.ManageDates.Ok);
            result.ViewModel.Should().Be(viewModel);
        }

        [Test]
        public void ShouldNotUpdateTheVacancyIfThereAreWarningsAndWeDontAcceptThem()
        {
            var vacancyReferenceNumber = 1;

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Now.AddDays(7)),
                PossibleStartDate = new DateViewModel(DateTime.Now.AddDays(7)),
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var mediator = GetMediator();

            var result = mediator.UpdateVacancy(viewModel, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.ManageDates.FailedValidation);
        }

        [Test]
        public void ShouldNotUpdateTheVacancyIfWeAcceptTheWarningsButTheyAreDifferentFromThePreviousOnes()
        {
            const int vacancyReferenceNumber = 1;
            const int oldWarningHash = -1011218820;

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Now.AddDays(20)),
                PossibleStartDate = new DateViewModel(DateTime.Now.AddDays(20)),
                VacancyReferenceNumber = vacancyReferenceNumber,
                WarningsHash = oldWarningHash
            };

            var mediator = GetMediator();

            var result = mediator.UpdateVacancy(viewModel, true);

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

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = closingDate,
                PossibleStartDate = possibleStartDate,
                VacancyReferenceNumber = vacancyReferenceNumber,
                WarningsHash = oldWarningHash,
                VacancyApplicationsState = state
            };

            var mediator = GetMediator();

            VacancyPostingProvider.Setup(p => p.UpdateVacancy(It.IsAny<VacancyDatesViewModel>())).Returns(viewModel);

            var result = mediator.UpdateVacancy(viewModel, true);

            result.Code.Should().Be(expectedCode);
            VacancyPostingProvider.Verify(
                p =>
                    p.UpdateVacancy(
                        It.Is<VacancyDatesViewModel>(
                            v =>
                                v.VacancyReferenceNumber == vacancyReferenceNumber 
                                && v.ClosingDate == closingDate 
                                && v.PossibleStartDate == possibleStartDate)));
        }
    }
}