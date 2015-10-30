namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System;
    using Common.Validators;
    using Common.ViewModels;
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
    }
}