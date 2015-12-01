namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Common.Validators;
    using FluentAssertions;
    using FluentValidation;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class PreviewVacancyViewModelServerValidatorTests
    {
        private VacancyResubmissionValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyResubmissionValidator();
        }

        [Test]
        public void ShouldLetChildViewModelsValidate()
        {
            var viewModel = new VacancyViewModel
            {
                NewVacancyViewModel = new NewVacancyViewModel(),
                VacancySummaryViewModel = new VacancySummaryViewModel(),
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel(),
                ResubmitOption = false
            };

            var result = _validator.Validate(viewModel);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
        }
    }
}