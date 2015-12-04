namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Builders;
    using Common.Validators;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    /// <summary>
    /// Testing business rules on page https://valtech-uk.atlassian.net/wiki/display/NAS/QA+a+vacancy#QAavacancy-Businessrulesforadvertisingvacancies
    /// </summary>
    [TestFixture]
    public class MandatoryWageTests
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

        [TestCase(45, WageUnit.Weekly, 15, false)] //3 pounds per hour
        [TestCase(195, WageUnit.Monthly, 15, false)] //3 pounds per hour
        [TestCase(2340, WageUnit.Annually, 15, false)] //3 pounds per hour
        [TestCase(49.35, WageUnit.Weekly, 15, false)] //3.29 pounds per hour
        [TestCase(213.85, WageUnit.Monthly, 15, false)] //3.29 pounds per hour
        [TestCase(2566.2, WageUnit.Annually, 15, false)] //3.29 pounds per hour
        [TestCase(49.5, WageUnit.Weekly, 15, true)] //3.30 pounds per hour
        [TestCase(214.5, WageUnit.Monthly, 15, true)] //3.30 pounds per hour
        [TestCase(2574, WageUnit.Annually, 15, true)] //3.30 pounds per hour
        public void ApprenticeMinimumWage_PerHour(decimal wage, WageUnit wageUnit, decimal hoursPerWeek, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                Wage = wage,
                WageType = WageType.Custom,
                WageUnit = wageUnit,
                HoursPerWeek = hoursPerWeek
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, viewModel, RuleSet);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, viewModel, RuleSet);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel, RuleSet);
            }
        }
    }
}