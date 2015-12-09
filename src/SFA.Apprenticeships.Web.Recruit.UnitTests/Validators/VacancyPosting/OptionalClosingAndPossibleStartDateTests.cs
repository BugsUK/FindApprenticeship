namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System;
    using Builders;
    using Common.Validators;
    using Common.ViewModels;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class OptionalClosingAndPossibleStartDateTests
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

        [Test]
        public void ClosingDateInThePastWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(-7))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void PossibleStartDateInThePastWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                PossibleStartDate = new DateViewModel(DateTime.Today.AddDays(-7))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void ClosingDateDateMustBeTwoWeeksInTheFutureWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(13))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void PossibleStartDateMustBeAfterClosingDateWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(28)),
                PossibleStartDate = new DateViewModel(DateTime.Today.AddDays(21))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void PossibleStartDateMustNotBeTheSameAsTheClosingDateWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(28)),
                PossibleStartDate = new DateViewModel(DateTime.Today.AddDays(28))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void ClosingDateNotAValidDateNoException()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel
                {
                    Day = 31,
                    Month = 2,
                    Year = 2015
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void PossibleStartNotAValidDateNoException()
        {
            var viewModel = new VacancySummaryViewModel
            {
                PossibleStartDate = new DateViewModel
                {
                    Day = 31,
                    Month = 2,
                    Year = 2015
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void ClosingDateAndPossibleStartNotAValidDateNoException()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel
                {
                    Day = 31,
                    Month = 2,
                    Year = 2015
                },
                PossibleStartDate = new DateViewModel
                {
                    Day = 31,
                    Month = 2,
                    Year = 2015
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void ClosingDateAndPossibleStartDateLessThanTwoWeeks()
        {
            var today = DateTime.Today;

            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(today),
                PossibleStartDate = new DateViewModel(today)
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var result = _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _validator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _validator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.ClosingDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.PossibleStartDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(2);
        }
    }
}