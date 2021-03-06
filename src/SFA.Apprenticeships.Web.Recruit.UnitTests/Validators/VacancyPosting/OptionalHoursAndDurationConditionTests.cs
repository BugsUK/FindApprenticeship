﻿namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System.Linq;
    using Builders;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Raa.Common.Constants.ViewModels;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    [TestFixture]
    [Parallelizable]
    public class OptionalHoursAndDurationConditionTests
    {
        private const string RuleSet = RuleSets.Warnings;

        private VacancySummaryViewModelServerValidator _validator;
        private VacancyViewModelValidator _aggregateValidator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
            _aggregateValidator = new VacancyViewModelValidator();
        }

        [TestCase(28, 57, DurationType.Weeks)]
        [TestCase(25, 63, DurationType.Weeks)]
        [TestCase(22, 72, DurationType.Weeks)]
        [TestCase(20, 78, DurationType.Weeks)]
        [TestCase(18, 87, DurationType.Weeks)]
        [TestCase(16, 98, DurationType.Weeks)]
        [TestCase(28, 13, DurationType.Months)]
        [TestCase(25, 15, DurationType.Months)]
        [TestCase(22, 17, DurationType.Months)]
        [TestCase(20, 18, DurationType.Months)]
        [TestCase(18, 20, DurationType.Months)]
        [TestCase(16, 23, DurationType.Months)]
        [TestCase(28, 2, DurationType.Years)]
        [TestCase(25, 2, DurationType.Years)]
        [TestCase(22, 2, DurationType.Years)]
        [TestCase(20, 2, DurationType.Years)]
        [TestCase(18, 2, DurationType.Years)]
        [TestCase(16, 2, DurationType.Years)]
        public void RuleFour_HoursPerWeek16to30_And_DurationGreaterThanOrEqualTo12months_And_ExpectedDurationGreaterThanOrEqualToMinimumDuration(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { CustomType = CustomWageType.Fixed, HoursPerWeek = hoursPerWeek },
                Duration = expectedDuration,
                DurationType = durationType
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.
            Duration, vacancyViewModel, RuleSet);
        }

        [TestCase(28, 56, DurationType.Weeks, VacancyViewModelMessages.Duration.DurationWarning28Hours)]
        [TestCase(25, 62, DurationType.Weeks, VacancyViewModelMessages.Duration.DurationWarning25Hours)]
        [TestCase(22, 71, DurationType.Weeks, VacancyViewModelMessages.Duration.DurationWarning22Hours)]
        [TestCase(20, 77, DurationType.Weeks, VacancyViewModelMessages.Duration.DurationWarning20Hours)]
        [TestCase(18, 86, DurationType.Weeks, VacancyViewModelMessages.Duration.DurationWarning18Hours)]
        [TestCase(16, 97, DurationType.Weeks, VacancyViewModelMessages.Duration.DurationWarning16Hours)]
        [TestCase(28, 12, DurationType.Months, VacancyViewModelMessages.Duration.DurationWarning28Hours)]
        [TestCase(25, 13, DurationType.Months, VacancyViewModelMessages.Duration.DurationWarning25Hours)]
        [TestCase(22, 16, DurationType.Months, VacancyViewModelMessages.Duration.DurationWarning22Hours)]
        [TestCase(20, 17, DurationType.Months, VacancyViewModelMessages.Duration.DurationWarning20Hours)]
        [TestCase(18, 19, DurationType.Months, VacancyViewModelMessages.Duration.DurationWarning18Hours)]
        [TestCase(16, 22, DurationType.Months, VacancyViewModelMessages.Duration.DurationWarning16Hours)]
        [TestCase(28, 1, DurationType.Years, VacancyViewModelMessages.Duration.DurationWarning28Hours)]
        [TestCase(25, 1, DurationType.Years, VacancyViewModelMessages.Duration.DurationWarning25Hours)]
        [TestCase(22, 1, DurationType.Years, VacancyViewModelMessages.Duration.DurationWarning22Hours)]
        [TestCase(20, 1, DurationType.Years, VacancyViewModelMessages.Duration.DurationWarning20Hours)]
        [TestCase(18, 1, DurationType.Years, VacancyViewModelMessages.Duration.DurationWarning18Hours)]
        [TestCase(16, 1, DurationType.Years, VacancyViewModelMessages.Duration.DurationWarning16Hours)]
        public void RuleFour_HoursPerWeek16to30_And_DurationGreaterThanOrEqualTo12months_And_ExpectedDurationLessThanMinimumDuration(decimal hoursPerWeek, int expectedDuration, DurationType durationType, string expectedMessage)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { HoursPerWeek = hoursPerWeek },
                Duration = expectedDuration,
                DurationType = durationType
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var response = _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            var aggregateResponse = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
            var error = response.Errors.SingleOrDefault(e => e.PropertyName == "Duration");
            error.Should().NotBeNull();
            error?.ErrorMessage.Should().Be(expectedMessage);
            var aggregateError = aggregateResponse.Errors.SingleOrDefault(e => e.PropertyName == "FurtherVacancyDetailsViewModel.Duration");
            aggregateError.Should().NotBeNull();
            aggregateError?.ErrorMessage.Should().Be(expectedMessage);
        }

        [TestCase(28, 56, DurationType.Weeks)]
        [TestCase(25, 62, DurationType.Weeks)]
        [TestCase(22, 71, DurationType.Weeks)]
        [TestCase(20, 77, DurationType.Weeks)]
        [TestCase(18, 86, DurationType.Weeks)]
        [TestCase(16, 97, DurationType.Weeks)]
        [TestCase(28, 12, DurationType.Months)]
        [TestCase(25, 13, DurationType.Months)]
        [TestCase(22, 16, DurationType.Months)]
        [TestCase(20, 17, DurationType.Months)]
        [TestCase(18, 19, DurationType.Months)]
        [TestCase(16, 22, DurationType.Months)]
        [TestCase(28, 1, DurationType.Years)]
        [TestCase(25, 1, DurationType.Years)]
        [TestCase(22, 1, DurationType.Years)]
        [TestCase(20, 1, DurationType.Years)]
        [TestCase(18, 1, DurationType.Years)]
        [TestCase(16, 1, DurationType.Years)]
        public void RuleFour_DoesNotApplyToTraineeships(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { HoursPerWeek = hoursPerWeek },
                Duration = expectedDuration,
                DurationType = durationType,
                VacancyType = VacancyType.Traineeship
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
        }

        [TestCase(41, 56, DurationType.Weeks)]
        [TestCase(15, 97, DurationType.Weeks)]
        [TestCase(41, 12, DurationType.Months)]
        [TestCase(15, 22, DurationType.Months)]
        [TestCase(41, 1, DurationType.Years)]
        [TestCase(15, 1, DurationType.Years)]
        [TestCase(-1, 56, DurationType.Weeks)]
        [TestCase(0, 97, DurationType.Weeks)]
        [TestCase(1, 97, DurationType.Weeks)]
        [TestCase(-1, 12, DurationType.Months)]
        [TestCase(0, 22, DurationType.Months)]
        [TestCase(1, 22, DurationType.Months)]
        [TestCase(-1, 1, DurationType.Years)]
        [TestCase(0, 1, DurationType.Years)]
        [TestCase(1, 1, DurationType.Years)]
        public void RuleFour_HoursPerWeekOutside16to30_And_DurationGreaterThanOrEqualTo12months_And_ExpectedDurationLessThanMinimumDuration(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { HoursPerWeek = hoursPerWeek },
                Duration = expectedDuration,
                DurationType = durationType
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
            //Other errors will superceed this warning so will be valid
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
        }
    }
}