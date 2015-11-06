namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class VacancySummaryViewModelClientValidatorTests
    {
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

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel);
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

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel);
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

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.LongDescription, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.LongDescription, viewModel);
            }
        }
    }
}