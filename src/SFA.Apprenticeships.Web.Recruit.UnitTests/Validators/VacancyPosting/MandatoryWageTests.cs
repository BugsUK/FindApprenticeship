namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Common.Validators;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    /// <summary>
    /// Testing business rules on page https://valtech-uk.atlassian.net/wiki/display/NAS/QA+a+vacancy#QAavacancy-Businessrulesforadvertisingvacancies
    /// </summary>
    [TestFixture]
    public class MandatoryWageTests
    {
        private const string RuleSet = RuleSets.Errors;

        private VacancySummaryViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, viewModel, RuleSet);
            }
        }
    }
}