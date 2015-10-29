using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System.Globalization;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    /// <summary>
    /// Testing business rules on page https://valtech-uk.atlassian.net/wiki/display/NAS/QA+a+vacancy#QAavacancy-Businessrulesforadvertisingvacancies
    /// </summary>
    [TestFixture]
    public class MandatoryHoursAndDurationConditionTests
    {
        private VacancySummaryViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
        }

        [TestCase(30, 1, DurationType.Years)]
        [TestCase(30, 10, DurationType.Years)]
        [TestCase(40, 1, DurationType.Years)]
        [TestCase(40, 10, DurationType.Years)]
        [TestCase(30, 12, DurationType.Months)]
        [TestCase(30, 120, DurationType.Months)]
        [TestCase(40, 12, DurationType.Months)]
        [TestCase(40, 120, DurationType.Months)]
        [TestCase(30, 52, DurationType.Weeks)]
        [TestCase(40, 52, DurationType.Weeks)]
        [TestCase(30, 520, DurationType.Weeks)]
        [TestCase(40, 520, DurationType.Weeks)]
        public void RuleOne_HoursPerWeek30to40_And_DurationGreaterOrEqualTo12months(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration,
                DurationType = durationType
            };

            _validator.Validate(viewModel);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel);
        }

        [TestCase(30, 0, DurationType.Years)]
        [TestCase(40, 0, DurationType.Years)]
        [TestCase(30, 11, DurationType.Months)]
        [TestCase(40, 11, DurationType.Months)]
        [TestCase(30, 51, DurationType.Weeks)]
        [TestCase(40, 51, DurationType.Weeks)]
        public void RuleTwo_HoursPerWeek30to40_And_DurationLessThan12months(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration,
                DurationType = durationType
            };

            _validator.Validate(viewModel);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
            _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel);
        }

        [TestCase(29, 13, DurationType.Months)]
        [TestCase(28, 13, DurationType.Months)]
        [TestCase(27, 14.5, DurationType.Months)]
        [TestCase(26, 14.5, DurationType.Months)]
        [TestCase(25, 14.5, DurationType.Months)]
        [TestCase(22, 16.5, DurationType.Months)]
        [TestCase(20, 18, DurationType.Months)]
        [TestCase(18, 20, DurationType.Months)]
        [TestCase(16, 22.5, DurationType.Months)]
        public void RuleThree_HoursPerWeek16to30_And_DurationGreaterThanOrEqualTo12months_And_ExpectedDurationGreaterThanOrEqualToMinimumDuration(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration,
                DurationType = durationType
            };

            _validator.Validate(viewModel);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel);
        }

        [TestCase(28, 12.5)]
        [TestCase(25, 14)]
        [TestCase(22, 16)]
        [TestCase(20, 17.5)]
        [TestCase(18, 29.5)]
        [TestCase(16, 22)]
        public void RuleFourTest_HoursLessThanThirtyDurationOutsideExpectedRange(decimal hoursPerWeek, int expectedDuration)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration
            };

            _validator.Validate(viewModel);

            //Assert. This rule will be a warning rather than being mandatory and so is not implemented by the VacancySummaryViewModelServerValidator
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel);
        }

        [TestCase(28, 11)]
        [TestCase(25, 11)]
        [TestCase(22, 11)]
        [TestCase(20, 11)]
        [TestCase(18, 11)]
        [TestCase(16, 11)]
        public void RuleFiveTest_HoursLessThanThirtyDurationOutsideExpectedRange(decimal hoursPerWeek, int expectedDuration)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration
            };

            _validator.Validate(viewModel);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
            _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel);
        }

        [Test]
        public void RuleSixTest_HoursLessThanSixteenDurationInsideExpectRange()
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = 15,
                Duration = 12
            };

            _validator.Validate(viewModel);

            _validator.ShouldHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel);
        }
    }
}