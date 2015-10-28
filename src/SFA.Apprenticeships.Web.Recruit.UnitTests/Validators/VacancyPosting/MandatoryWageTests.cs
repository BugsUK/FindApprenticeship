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
    [TestFixture, Ignore]
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
        public void ApprenticeMinimumWage_PerHour(double wage, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                WeeklyWage = wage.ToString(CultureInfo.InvariantCulture),
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.WeeklyWage, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WeeklyWage, viewModel);
            }
        }
    }
}