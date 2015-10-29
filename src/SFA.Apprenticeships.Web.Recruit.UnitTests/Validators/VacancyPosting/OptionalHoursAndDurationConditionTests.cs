namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Common.Validators;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class OptionalHoursAndDurationConditionTests
    {
        private const string RuleSet = RuleSets.Warnings;

        private VacancySummaryViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
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
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration,
                DurationType = durationType
            };

            _validator.Validate(viewModel, RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
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
        public void RuleFour_HoursPerWeek16to30_And_DurationGreaterThanOrEqualTo12months_And_ExpectedDurationLessThanMinimumDuration(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration,
                DurationType = durationType
            };

            _validator.Validate(viewModel, RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
        }
    }
}