namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Common.Validators;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class VacancySummaryViewModelClientValidatorTests
    {
        private const string RuleSet = RuleSets.Errors;

        private VacancySummaryViewModelClientValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelClientValidator();
        }

        [Test]
        public void DefaultShouldNotHaveAnyValidationErrors()
        {
            var viewModel = new VacancySummaryViewModel();

            var result = _validator.Validate(viewModel);

            result.IsValid.Should().BeTrue();
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void WorkingWeekInvalidCharacters(string workingWeek, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                WorkingWeek = workingWeek
            };

            _validator.Validate(viewModel, RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel, RuleSet);
            }
        }

        [TestCase("Working Week", true)]
        [TestCase("More than 256 characters. More than 256 characters. More than 256 characters. More than 256 characters. More than 256 characters. More than 256 characters. More than 256 characters. More than 256 characters. More than 256 characters. More than 256 characters.", false)]
        public void WorkingWeekLength(string workingWeek, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                WorkingWeek = workingWeek
            };

            _validator.Validate(viewModel, RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel, RuleSet);
            }
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void LongDescriptionInvalidCharacters(string longDescription, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                LongDescription = longDescription
            };

            _validator.Validate(viewModel, RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.LongDescription, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.LongDescription, viewModel, RuleSet);
            }
        }
    }
}