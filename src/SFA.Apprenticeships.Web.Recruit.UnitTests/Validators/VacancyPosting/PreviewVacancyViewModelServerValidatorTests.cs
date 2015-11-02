namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Common.Validators;
    using FluentAssertions;
    using FluentValidation;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class PreviewVacancyViewModelServerValidatorTests
    {
        private VacancyViewModelValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyViewModelValidator();
        }

        [Test]
        public void ShouldLetChildViewModelsValidate()
        {
            var viewModel = new VacancyViewModel
            {
                NewVacancyViewModel = new NewVacancyViewModel(),
                VacancySummaryViewModel = new VacancySummaryViewModel(),
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };

            var result = _validator.Validate(viewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().BeGreaterThan(5);
        }
    }
}