﻿namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System;
    using System.Linq;
    using Builders;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    [Parallelizable]
    public class MandatoryDateChecks
    {
        private const string RuleSet = RuleSets.Errors;

        private VacancySummaryViewModelServerValidator _validator;
        private VacancyViewModelValidator _aggregateValidator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
            _aggregateValidator = new VacancyViewModelValidator();
        }

        [Test]
        public void ClosingDateTwoWeeksAway_PossibleStartDateAfterClosingDate()
        {
            var today = DateTime.Today;

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(today.AddDays(14)),
                    PossibleStartDate = new DateViewModel(today.AddDays(15))
                },
                Wage = new WageViewModel()
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.ClosingDate, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void ClosingDateLessThanTwoWeeksAway_PossibleStartDateBeforeClosingDate()
        {
            var today = DateTime.Today;

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(today.AddDays(13)),
                    PossibleStartDate = new DateViewModel(today.AddDays(12))
                },
                Wage = new WageViewModel()
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            //Assert. This rule will be a warning rather than being mandatory and so is not implemented by the VacancySummaryViewModelServerValidator
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.ClosingDate, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }

        [TestCase(VacancyStatus.Unknown, VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)]
        [TestCase(VacancyStatus.Draft, VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)]
        [TestCase(VacancyStatus.Live, VacancyViewModelMessages.ClosingDate.TodayOrInTheFutureErrorText)]
        [TestCase(VacancyStatus.Referred, VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)]
        [TestCase(VacancyStatus.Deleted, VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)]
        [TestCase(VacancyStatus.Submitted, VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)]
        [TestCase(VacancyStatus.Closed, VacancyViewModelMessages.ClosingDate.TodayOrInTheFutureErrorText)]
        [TestCase(VacancyStatus.Withdrawn, VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)]
        [TestCase(VacancyStatus.Completed, VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)]
        [TestCase(VacancyStatus.PostedInError, VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)]
        [TestCase(VacancyStatus.ReservedForQA, VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)]
        public void DateCannotBeYesterday(VacancyStatus vacancyStatus, string expectedMessage)
        {
            var yesterday = DateTime.Today.AddDays(-1);

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(yesterday),
                    PossibleStartDate = new DateViewModel(yesterday),
                    VacancyStatus = vacancyStatus
                },
                Wage = new WageViewModel()
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var response = _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            var aggregateResponse = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.VacancyDatesViewModel, vm => vm.VacancyDatesViewModel.ClosingDate, viewModel, RuleSet);
            _validator.ShouldHaveValidationErrorFor(vm => vm.VacancyDatesViewModel, vm => vm.VacancyDatesViewModel.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSet);

            var closingDateError = response.Errors.SingleOrDefault(e => e.PropertyName == "VacancyDatesViewModel.ClosingDate");
            closingDateError.Should().NotBeNull();
            closingDateError?.ErrorMessage.Should().Be(expectedMessage);
            var closingDateAggregateError = aggregateResponse.Errors.SingleOrDefault(e => e.PropertyName == "FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate");
            closingDateAggregateError.Should().NotBeNull();
            closingDateAggregateError?.ErrorMessage.Should().Be(expectedMessage);

            var possibleStartDateError = response.Errors.SingleOrDefault(e => e.PropertyName == "VacancyDatesViewModel.PossibleStartDate");
            possibleStartDateError.Should().NotBeNull();
            possibleStartDateError?.ErrorMessage.Should().Be(VacancyViewModelMessages.PossibleStartDate.AfterTodayErrorText);
            var possibleStartDateAggregateError = aggregateResponse.Errors.SingleOrDefault(e => e.PropertyName == "FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate");
            possibleStartDateAggregateError.Should().NotBeNull();
            possibleStartDateAggregateError?.ErrorMessage.Should().Be(VacancyViewModelMessages.PossibleStartDate.AfterTodayErrorText);
        }

        [TestCase(VacancyStatus.Unknown, false)]
        [TestCase(VacancyStatus.Draft, false)]
        [TestCase(VacancyStatus.Live, true)]
        [TestCase(VacancyStatus.Referred, false)]
        [TestCase(VacancyStatus.Deleted, false)]
        [TestCase(VacancyStatus.Submitted, false)]
        [TestCase(VacancyStatus.Closed, true)]
        [TestCase(VacancyStatus.Withdrawn, false)]
        [TestCase(VacancyStatus.Completed, false)]
        [TestCase(VacancyStatus.PostedInError, false)]
        [TestCase(VacancyStatus.ReservedForQA, false)]
        public void ClosingDateCanBeTodayIfLive(VacancyStatus vacancyStatus, bool expectValid)
        {
            var today = DateTime.Today;

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(today),
                    VacancyStatus = vacancyStatus
                },
                Wage = new WageViewModel()
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var response = _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            var aggregateResponse = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyDatesViewModel,
                    vm => vm.VacancyDatesViewModel.ClosingDate, viewModel, RuleSet);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel,
                    vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel,
                    vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel,
                    vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel,
                    vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.VacancyDatesViewModel,
                    vm => vm.VacancyDatesViewModel.ClosingDate, viewModel, RuleSet);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel,
                    vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel,
                    vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel,
                    vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel,
                    vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);

                var error = response.Errors.SingleOrDefault(e => e.PropertyName == "VacancyDatesViewModel.ClosingDate");
                error.Should().NotBeNull();
                error?.ErrorMessage.Should().Be(VacancyViewModelMessages.ClosingDate.AfterTodayErrorText);
                var aggregateError = aggregateResponse.Errors.SingleOrDefault(e => e.PropertyName == "FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate");
                aggregateError.Should().NotBeNull();
                aggregateError?.ErrorMessage.Should().Be(VacancyViewModelMessages.ClosingDate.AfterTodayErrorText);
            }
        }
    }
}