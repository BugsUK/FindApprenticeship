namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class VacancyQuestionsViewModelServerValidatorTests
    {
        private VacancyQuestionsViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyQuestionsViewModelServerValidator();
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        [TestCase("First Question", true)]
        public void FirstQuestionNotRequired(string firstQuestion, bool expectValid)
        {
            var viewModel = new VacancyQuestionsViewModel
            {
                FirstQuestion = firstQuestion
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.FirstQuestion, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.FirstQuestion, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        [TestCase("Second Question", true)]
        public void SecondQuestionNotRequired(string secondQuestion, bool expectValid)
        {
            var viewModel = new VacancyQuestionsViewModel
            {
                SecondQuestion = secondQuestion
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.SecondQuestion, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.SecondQuestion, viewModel);
            }
        }
    }
}