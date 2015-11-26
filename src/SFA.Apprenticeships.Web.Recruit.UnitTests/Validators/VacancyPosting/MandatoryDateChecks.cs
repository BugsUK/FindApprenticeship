namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System;
    using Builders;
    using Common.Validators;
    using Common.ViewModels;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
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
            var today = DateTime.UtcNow;
            today = new DateTime(today.Year, today.Month, today.Day);

            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(today.AddDays(14)),
                PossibleStartDate = new DateViewModel(today.AddDays(15))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void ClosingDateLessThanTwoWeeksAway_PossibleStartDateBeforeClosingDate()
        {
            var today = DateTime.UtcNow;
            today = new DateTime(today.Year, today.Month, today.Day);

            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(today.AddDays(13)),
                PossibleStartDate = new DateViewModel(today.AddDays(12))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            //Assert. This rule will be a warning rather than being mandatory and so is not implemented by the VacancySummaryViewModelServerValidator
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }
    }
}