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
    public class MandatoryWageTests
    {
        private VacancySummaryViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(3.29, false)]
        [TestCase(3.30, true)]
        [TestCase(3.31, true)]
        public void ApprenticeMinimumWage_PerHour(decimal wage, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                Wage = wage
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, viewModel);
            }
        }
    }
}