namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System;
    using System.Linq;
    using Builders;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using FluentValidation;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    /// <summary>
    /// Testing business rules on page https://valtech-uk.atlassian.net/wiki/display/NAS/QA+a+vacancy#QAavacancy-Businessrulesforadvertisingvacancies
    /// </summary>
    [TestFixture]
    [Parallelizable]
    public class MandatoryWageTests
    {
        private const string RuleSet = RuleSets.Errors;

        private VacancySummaryViewModelDatesServerValidator _validator;
        private VacancyViewModelValidator _aggregateValidator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelDatesServerValidator();
            _aggregateValidator = new VacancyViewModelValidator();
        }

        [TestCase(45, WageUnit.Weekly, 15, false)] //3 pounds per hour
        [TestCase(195, WageUnit.Monthly, 15, false)] //3 pounds per hour
        [TestCase(2340, WageUnit.Annually, 15, false)] //3 pounds per hour
        [TestCase(49.35, WageUnit.Weekly, 15, false)] //3.29 pounds per hour
        [TestCase(213.85, WageUnit.Monthly, 15, false)] //3.29 pounds per hour
        [TestCase(2566.2, WageUnit.Annually, 15, false)] //3.29 pounds per hour
        [TestCase(49.5, WageUnit.Weekly, 15, true)] //3.30 pounds per hour
        [TestCase(214.5, WageUnit.Monthly, 15, true)] //3.30 pounds per hour
        [TestCase(2574, WageUnit.Annually, 15, true)] //3.30 pounds per hour
        public void ApprenticeMinimumWage_PerHour_BeforeOctFirst2016(decimal wage, WageUnit wageUnit, decimal hoursPerWeek, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = WageClassification.Custom, CustomType = CustomWageType.Fixed, Amount = wage, Unit = wageUnit, HoursPerWeek = hoursPerWeek },
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    PossibleStartDate = new DateViewModel(new DateTime(2016, 9, 30))
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var response = _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            var aggregateResponse = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel, RuleSet);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel, RuleSet);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSet);

                var error = response.Errors.SingleOrDefault(e => e.PropertyName == "Wage.Amount");
                error.Should().NotBeNull();
                error?.ErrorMessage.Should().Be("The wage should not be less than the National Minimum Wage for apprentices");
                var aggregateError = aggregateResponse.Errors.SingleOrDefault(e => e.PropertyName == "FurtherVacancyDetailsViewModel.Wage.Amount");
                aggregateError.Should().NotBeNull();
                aggregateError?.ErrorMessage.Should().Be("The wage should not be less than the National Minimum Wage for apprentices");
            }
        }

        [TestCase(49.35, WageUnit.Weekly, 15, false)] //3.29 pounds per hour
        [TestCase(213.85, WageUnit.Monthly, 15, false)] //3.29 pounds per hour
        [TestCase(2566.2, WageUnit.Annually, 15, false)] //3.29 pounds per hour
        [TestCase(50.85, WageUnit.Weekly, 15, false)] //3.39 pounds per hour
        [TestCase(220.35, WageUnit.Monthly, 15, false)] //3.39 pounds per hour
        [TestCase(2644.2, WageUnit.Annually, 15, false)] //3.39 pounds per hour
        [TestCase(51, WageUnit.Weekly, 15, true)] //3.40 pounds per hour
        [TestCase(221, WageUnit.Monthly, 15, true)] //3.40 pounds per hour
        [TestCase(2652, WageUnit.Annually, 15, true)] //3.40 pounds per hour
        public void ApprenticeMinimumWage_PerHour_AfterOctFirst2016(decimal wage, WageUnit wageUnit, decimal hoursPerWeek, bool expectValid)
        {
            //After 1st of october 2016 the National Minimum Wage for Apprentices increases to £3.40/hour
            var viewModel = new FurtherVacancyDetailsViewModel
            {
				Wage = new WageViewModel() { Classification = WageClassification.Custom, CustomType = CustomWageType.Fixed, Amount = wage, Unit = wageUnit, HoursPerWeek = hoursPerWeek },
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    PossibleStartDate = new DateViewModel(new DateTime(2016, 10, 1))
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var response = _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            var aggregateResponse = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel, RuleSet);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel, RuleSet);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSet);

                var error = response.Errors.SingleOrDefault(e => e.PropertyName == "Wage.Amount");
                error.Should().NotBeNull();
                error?.ErrorMessage.Should().Be("The wage should not be less then the new National Minimum Wage for apprentices effective from 1 Oct 2016");
                var aggregateError = aggregateResponse.Errors.SingleOrDefault(e => e.PropertyName == "FurtherVacancyDetailsViewModel.Wage.Amount");
                aggregateError.Should().NotBeNull();
                aggregateError?.ErrorMessage.Should().Be("The wage should not be less then the new National Minimum Wage for apprentices effective from 1 Oct 2016");
            }
        }

        [TestCase(50.85, WageUnit.Weekly, 15, false)] //3.39 pounds per hour
        [TestCase(220.35, WageUnit.Monthly, 15, false)] //3.39 pounds per hour
        [TestCase(2644.2, WageUnit.Annually, 15, false)] //3.39 pounds per hour
        [TestCase(52.35, WageUnit.Weekly, 15, false)] //3.49 pounds per hour
        [TestCase(226.85, WageUnit.Monthly, 15, false)] //3.49 pounds per hour
        [TestCase(2722.2, WageUnit.Annually, 15, false)] //3.49 pounds per hour
        [TestCase(52.5, WageUnit.Weekly, 15, true)] //3.50 pounds per hour
        [TestCase(227.5, WageUnit.Monthly, 15, true)] //3.50 pounds per hour
        [TestCase(2730, WageUnit.Annually, 15, true)] //3.50 pounds per hour
        public void ApprenticeMinimumWage_PerHour_AfterAprilFirst2017(decimal wage, WageUnit wageUnit, decimal hoursPerWeek, bool expectValid)
        {
            //After 1st of october 2016 the National Minimum Wage for Apprentices increases to £3.40/hour
            var viewModel = new FurtherVacancyDetailsViewModel
            {
				Wage = new WageViewModel() { Classification = WageClassification.Custom, CustomType = CustomWageType.Fixed, Amount = wage, Unit = wageUnit, HoursPerWeek = hoursPerWeek },
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    PossibleStartDate = new DateViewModel(new DateTime(2017, 04, 1))
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var response = _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            var aggregateResponse = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel, RuleSet);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel, RuleSet);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSet);

                var error = response.Errors.SingleOrDefault(e => e.PropertyName == "Wage.Amount");
                error.Should().NotBeNull();
                error?.ErrorMessage.Should().Be("The wage should not be less then the new National Minimum Wage for apprentices effective from 1 Apr 2017");
                var aggregateError = aggregateResponse.Errors.SingleOrDefault(e => e.PropertyName == "FurtherVacancyDetailsViewModel.Wage.Amount");
                aggregateError.Should().NotBeNull();
                aggregateError?.ErrorMessage.Should().Be("The wage should not be less then the new National Minimum Wage for apprentices effective from 1 Apr 2017");
            }
        }

        [Test]
        public void ApprenticeMinimumWage_AfterOctFirst_DivideByZero()
        {
            //After 1st of october 2016 the National Minimum Wage for Apprentices increases to £3.40/hour
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { CustomType = CustomWageType.Fixed, Amount = 123.45m, Unit = WageUnit.Weekly, HoursPerWeek = 0 },
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    PossibleStartDate = new DateViewModel(new DateTime(2016, 10, 1))
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            Action validate = () => _validator.Validate(viewModel, ruleSet: RuleSet);
            Action aggregateValidate = () => _aggregateValidator.Validate(vacancyViewModel);

            validate.ShouldNotThrow();
            aggregateValidate.ShouldNotThrow();
        }
    }
}