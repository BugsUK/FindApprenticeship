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

        [TestCase(30, 12)]
        [TestCase(40, 12)]
        public void RuleOneTest_HoursAndDurationInsideExpectedRange(double hoursPerWeek, double expectedDuration)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration.ToString(CultureInfo.InvariantCulture)
            };

            _validator.Validate(viewModel);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel);
        }

        [TestCase(30, 11)]
        [TestCase(40, 11)]
        public void RuleTwoTest_HoursInsideExpectedRangeDurationOutsideExpectedRange(double hoursPerWeek, double expectedDuration)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration.ToString(CultureInfo.InvariantCulture)
            };

            _validator.Validate(viewModel);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
            _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel);
        }

        [TestCase(28, 13)]
        [TestCase(25, 14.5)]
        [TestCase(22, 16.5)]
        [TestCase(20, 18)]
        [TestCase(18, 20)]
        [TestCase(16, 22.5)]
        public void RuleThreeTest_HoursLessThanThirtyDurationInsideExpectRange(double hoursPerWeek, double expectedDuration)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration.ToString(CultureInfo.InvariantCulture)
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
        public void RuleFourTest_HoursLessThanThirtyDurationOutsideExpectedRange(double hoursPerWeek, double expectedDuration)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration.ToString(CultureInfo.InvariantCulture)
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
        public void RuleFiveTest_HoursLessThanThirtyDurationOutsideExpectedRange(double hoursPerWeek, double expectedDuration)
        {
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek,
                Duration = expectedDuration.ToString(CultureInfo.InvariantCulture)
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
                Duration = "12"
            };

            _validator.Validate(viewModel);

            _validator.ShouldHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel);
        }
    }
}