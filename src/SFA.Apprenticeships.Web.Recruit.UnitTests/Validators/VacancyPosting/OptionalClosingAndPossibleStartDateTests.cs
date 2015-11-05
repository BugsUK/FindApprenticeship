namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System;
    using Common.Validators;
    using Common.ViewModels;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class OptionalClosingAndPossibleStartDateTests
    {
        private const string RuleSet = RuleSets.Warnings;

        private VacancySummaryViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
        }

        [Test]
        public void ClosingDateInThePastWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(-7))
            };

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
        }

        [Test]
        public void PossibleStartDateInThePastWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                PossibleStartDate = new DateViewModel(DateTime.Today.AddDays(-7))
            };

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
        }

        [Test]
        public void ClosingDateDateMustBeTwoWeeksInTheFutureWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(13))
            };

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
        }

        [Test]
        public void PossibleStartDateMustBeAfterClosingDateWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(28)),
                PossibleStartDate = new DateViewModel(DateTime.Today.AddDays(21))
            };

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
        }

        [Test]
        public void PossibleStartDateMustNotBeTheSameAsTheClosingDateWarning()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(28)),
                PossibleStartDate = new DateViewModel(DateTime.Today.AddDays(28))
            };

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
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

            var result = _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _validator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(2);
        }
    }
}