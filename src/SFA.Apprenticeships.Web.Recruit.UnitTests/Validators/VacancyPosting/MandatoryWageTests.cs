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

    /// <summary>
    /// Testing business rules on page https://valtech-uk.atlassian.net/wiki/display/NAS/QA+a+vacancy#QAavacancy-Businessrulesforadvertisingvacancies
    /// </summary>
    [TestFixture]
    [Parallelizable]
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
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                WageObject = new WageViewModel(WageType.Custom, wage, null, wageUnit, hoursPerWeek),
                Wage = wage,
                HoursPerWeek = hoursPerWeek
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, viewModel, RuleSet);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vacancyViewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, viewModel, RuleSet);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vacancyViewModel, RuleSet);
            }
        }
    }
}