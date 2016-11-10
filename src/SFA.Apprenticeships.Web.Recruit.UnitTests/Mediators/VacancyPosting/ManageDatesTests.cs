namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using Builders;
    using Common.Constants;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    [TestFixture]
    [Parallelizable]
    public class ManageDatesTests : TestsBase
    {
        [Test]
        public void ShouldMergeRequestAndExistingFurtherVacancyDetails()
        {
            const int vacancyReferenceNumber = 1;

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                Wage = new WageViewModel() {
                    CustomType = CustomWageType.Fixed,
                    Amount = 99, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.Weekly, HoursPerWeek = 30 },
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.Now.AddDays(7)),
                    PossibleStartDate = new DateViewModel(DateTime.Now.AddDays(7))
                }
            };

            var existingViewModel = new VacancyViewModelBuilder().With(new WageViewModel() { Type = WageType.NationalMinimum, Classification = WageClassification.NationalMinimum, CustomType = CustomWageType.NotApplicable, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.Weekly, HoursPerWeek = 37.5m}).BuildValid(VacancyStatus.Live, VacancyType.Apprenticeship).FurtherVacancyDetailsViewModel;

            VacancyPostingProvider.Setup(p => p.GetVacancySummaryViewModel(vacancyReferenceNumber)).Returns(existingViewModel);

            var mediator = GetMediator();

            var result = mediator.UpdateVacancyDates(viewModel, false);

            VacancyPostingProvider.Verify(p => p.GetVacancySummaryViewModel(vacancyReferenceNumber));

            result.ViewModel.Wage.Type.Should().Be(viewModel.Wage.Type);
            result.ViewModel.Wage.Amount.Should().Be(viewModel.Wage.Amount);
            result.ViewModel.Wage.Text.Should().Be(existingViewModel.Wage.Text);
            result.ViewModel.Wage.Unit.Should().Be(viewModel.Wage.Unit);
            result.ViewModel.Wage.HoursPerWeek.Should().Be(existingViewModel.Wage.HoursPerWeek);
            result.ViewModel.VacancyDatesViewModel.ClosingDate.Should().Be(viewModel.VacancyDatesViewModel.ClosingDate);
            result.ViewModel.VacancyDatesViewModel.PossibleStartDate.Should().Be(viewModel.VacancyDatesViewModel.PossibleStartDate);
        }

        [Test]
        public void ShouldReturnAWarningHashIfTheModelHasOnlyWarnings()
        {
            const int vacancyReferenceNumber = 1;

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Type = WageType.NationalMinimum, Classification = WageClassification.NationalMinimum, CustomType = CustomWageType.NotApplicable, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.Weekly, HoursPerWeek = 30 },
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
                Wage = new WageViewModel() { Type = WageType.NationalMinimum, Classification = WageClassification.NationalMinimum, CustomType = CustomWageType.NotApplicable, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.NotApplicable, HoursPerWeek = null },
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
                Wage = new WageViewModel() { Type = WageType.NationalMinimum, Classification = WageClassification.NationalMinimum, CustomType = CustomWageType.NotApplicable, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.NotApplicable, HoursPerWeek = null },
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

        [Test]
        public void FailedMinimumWageValidation()
        {
            var closingDate = DateTime.Now.AddDays(7);
            var minimumWageChangeDate = new DateTime(2016, 10, 1);
            if (closingDate < minimumWageChangeDate)
            {
                closingDate = minimumWageChangeDate;
            }

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(closingDate),
                    PossibleStartDate = new DateViewModel(closingDate.AddDays(7))
                }
            };

            var existingViewModel = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Live, VacancyType.Apprenticeship).FurtherVacancyDetailsViewModel;
            existingViewModel.ComeFromPreview = false;
            //Invalid after Sept 30th
            existingViewModel.Wage.Type = WageType.Custom;
            existingViewModel.Wage.Classification = WageClassification.Custom;
            existingViewModel.Wage.CustomType = CustomWageType.Fixed;
            existingViewModel.Wage.Amount = 99;
            existingViewModel.Wage.Unit = WageUnit.Weekly;
            existingViewModel.Wage.HoursPerWeek = 30;

            VacancyPostingProvider.Setup(p => p.GetVacancySummaryViewModel(It.IsAny<int>())).Returns(existingViewModel);

            var mediator = GetMediator();

            var result = mediator.UpdateVacancyDates(viewModel, false);
            result.ViewModel.ComeFromPreview.Should().BeTrue();
            result.Code.Should().Be(VacancyPostingMediatorCodes.ManageDates.FailedCrossFieldValidation);
            result.Message.Level.Should().Be(UserMessageLevel.Warning);
            result.Message.Text.Should().Be(VacancyViewModelMessages.FailedCrossFieldValidation);
        }
    }
}