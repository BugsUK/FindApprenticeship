namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class VacancyQuestionsViewModelClientValidatorTests
    {
        private VacancyQuestionsViewModelClientValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyQuestionsViewModelClientValidator();
        }

        [Test]
        public void DefaultShouldNotHaveAnyValidationErrors()
        {
            var viewModel = new VacancyQuestionsViewModel();

            var result = _validator.Validate(viewModel);

            result.IsValid.Should().BeTrue();
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void FirstQuestionInvalidCharacters(string firstQuestion, bool expectValid)
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
        public void SecondQuestionInvalidCharacters(string secondQuestion, bool expectValid)
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