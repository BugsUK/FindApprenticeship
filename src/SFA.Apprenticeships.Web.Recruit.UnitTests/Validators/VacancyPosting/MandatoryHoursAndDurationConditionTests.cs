namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Builders;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    /// <summary>
    /// Testing business rules on page https://valtech-uk.atlassian.net/wiki/display/NAS/QA+a+vacancy#QAavacancy-Businessrulesforadvertisingvacancies
    /// </summary>
    [TestFixture]
    [Parallelizable]
    public class MandatoryHoursAndDurationConditionTests
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
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(WageType.Custom, null, null, WageUnit.NotApplicable, hoursPerWeek),
                Duration = expectedDuration,
                DurationType = durationType
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
        }

        [TestCase(30, 0, DurationType.Years)]
        [TestCase(40, 0, DurationType.Years)]
        [TestCase(30, 11, DurationType.Months)]
        [TestCase(40, 11, DurationType.Months)]
        [TestCase(30, 51, DurationType.Weeks)]
        [TestCase(40, 51, DurationType.Weeks)]
        public void RuleTwo_HoursPerWeek30to40_And_DurationLessThan12months(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(WageType.Custom, null, null, WageUnit.NotApplicable, hoursPerWeek),
                Duration = expectedDuration,
                DurationType = durationType,
                VacancySource = VacancySource.Raa
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
        }

        [TestCase(29, 57, DurationType.Weeks)]
        [TestCase(28, 57, DurationType.Weeks)]
        [TestCase(27, 65, DurationType.Weeks)]
        [TestCase(26, 65, DurationType.Weeks)]
        [TestCase(25, 65, DurationType.Weeks)]
        [TestCase(22, 74, DurationType.Weeks)]
        [TestCase(20, 78, DurationType.Weeks)]
        [TestCase(18, 87, DurationType.Weeks)]
        [TestCase(16, 100, DurationType.Weeks)]
        [TestCase(29, 2, DurationType.Years)]
        [TestCase(28, 2, DurationType.Years)]
        [TestCase(27, 2, DurationType.Years)]
        [TestCase(26, 2, DurationType.Years)]
        [TestCase(25, 2, DurationType.Years)]
        [TestCase(22, 2, DurationType.Years)]
        [TestCase(20, 2, DurationType.Years)]
        [TestCase(18, 2, DurationType.Years)]
        [TestCase(16, 2, DurationType.Years)]
        [TestCase(29, 13, DurationType.Months)]
        [TestCase(28, 13, DurationType.Months)]
        [TestCase(27, 15, DurationType.Months)]
        [TestCase(26, 15, DurationType.Months)]
        [TestCase(25, 15, DurationType.Months)]
        [TestCase(22, 17, DurationType.Months)]
        [TestCase(20, 18, DurationType.Months)]
        [TestCase(18, 20, DurationType.Months)]
        [TestCase(16, 23, DurationType.Months)]
        public void RuleThree_HoursPerWeek16to30_And_DurationGreaterThanOrEqualTo12months_And_ExpectedDurationGreaterThanOrEqualToMinimumDuration(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(WageType.Custom, null, null, WageUnit.NotApplicable, hoursPerWeek),
                Duration = expectedDuration,
                DurationType = durationType
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
        }

        [TestCase(29, 56, DurationType.Weeks)]
        [TestCase(28, 56, DurationType.Weeks)]
        [TestCase(27, 64, DurationType.Weeks)]
        [TestCase(26, 64, DurationType.Weeks)]
        [TestCase(25, 64, DurationType.Weeks)]
        [TestCase(22, 73, DurationType.Weeks)]
        [TestCase(20, 77, DurationType.Weeks)]
        [TestCase(18, 87, DurationType.Weeks)]
        [TestCase(16, 99, DurationType.Weeks)]
        [TestCase(29, 1, DurationType.Years)]
        [TestCase(28, 1, DurationType.Years)]
        [TestCase(27, 1, DurationType.Years)]
        [TestCase(26, 1, DurationType.Years)]
        [TestCase(25, 1, DurationType.Years)]
        [TestCase(22, 1, DurationType.Years)]
        [TestCase(20, 1, DurationType.Years)]
        [TestCase(18, 1, DurationType.Years)]
        [TestCase(16, 1, DurationType.Years)]
        [TestCase(29, 12, DurationType.Months)]
        [TestCase(28, 12, DurationType.Months)]
        [TestCase(27, 14, DurationType.Months)]
        [TestCase(26, 14, DurationType.Months)]
        [TestCase(25, 14, DurationType.Months)]
        [TestCase(22, 16, DurationType.Months)]
        [TestCase(20, 17, DurationType.Months)]
        [TestCase(18, 19, DurationType.Months)]
        [TestCase(16, 22, DurationType.Months)]
        public void RuleFour_HoursPerWeek16to30_And_DurationGreaterThanOrEqualTo12months_And_ExpectedDurationNotGreaterThanOrEqualToMinimumDuration(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(WageType.Custom, null, null, WageUnit.NotApplicable, hoursPerWeek),
                Duration = expectedDuration,
                DurationType = durationType
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            //Assert. This rule will be a warning rather than being mandatory and so is not implemented by the VacancySummaryViewModelServerValidator
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
        }

        [TestCase(29, 51, DurationType.Weeks)]
        [TestCase(28, 51, DurationType.Weeks)]
        [TestCase(27, 51, DurationType.Weeks)]
        [TestCase(26, 51, DurationType.Weeks)]
        [TestCase(25, 51, DurationType.Weeks)]
        [TestCase(22, 51, DurationType.Weeks)]
        [TestCase(20, 51, DurationType.Weeks)]
        [TestCase(18, 51, DurationType.Weeks)]
        [TestCase(16, 51, DurationType.Weeks)]
        [TestCase(29, 0, DurationType.Years)]
        [TestCase(28, 0, DurationType.Years)]
        [TestCase(27, 0, DurationType.Years)]
        [TestCase(26, 0, DurationType.Years)]
        [TestCase(25, 0, DurationType.Years)]
        [TestCase(22, 0, DurationType.Years)]
        [TestCase(20, 0, DurationType.Years)]
        [TestCase(18, 0, DurationType.Years)]
        [TestCase(16, 0, DurationType.Years)]
        [TestCase(29, 11, DurationType.Months)]
        [TestCase(28, 11, DurationType.Months)]
        [TestCase(27, 11, DurationType.Months)]
        [TestCase(26, 11, DurationType.Months)]
        [TestCase(25, 11, DurationType.Months)]
        [TestCase(22, 11, DurationType.Months)]
        [TestCase(20, 11, DurationType.Months)]
        [TestCase(18, 11, DurationType.Months)]
        [TestCase(16, 11, DurationType.Months)]
        public void RuleFive_HoursPerWeek16to30_And_DurationNotGreaterThanOrEqualTo12months(decimal hoursPerWeek, int expectedDuration, DurationType durationType)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(WageType.Custom, null, null, WageUnit.NotApplicable, hoursPerWeek),
                Duration = expectedDuration,
                DurationType = durationType,
                VacancySource = VacancySource.Raa
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(15)]
        public void RuleSix_HoursPerWeekLessThan16OrGreaterThan40(decimal hoursPerWeek)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(WageType.Custom, null, null, WageUnit.NotApplicable, hoursPerWeek)
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSet);
        }

        [TestCase(5, DurationType.Weeks, false)]
        [TestCase(6, DurationType.Weeks, true)]
        [TestCase(26, DurationType.Weeks, true)]
        [TestCase(27, DurationType.Weeks, false)]
        [TestCase(1, DurationType.Months, false)]
        [TestCase(2, DurationType.Months, true)]
        [TestCase(6, DurationType.Months, true)]
        [TestCase(7, DurationType.Months, false)]
        public void TraineeshipDuration(int expectedDuration, DurationType durationType, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Duration = expectedDuration,
                DurationType = durationType,
                VacancyType = VacancyType.Traineeship,
                VacancySource = VacancySource.Raa,
                Wage = new WageViewModel(WageType.Custom, null, null, WageUnit.NotApplicable, null)
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            if(expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSet);
            }
        }
    }
}
