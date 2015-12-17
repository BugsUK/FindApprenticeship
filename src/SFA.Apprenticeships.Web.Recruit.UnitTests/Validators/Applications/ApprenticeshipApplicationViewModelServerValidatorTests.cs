namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.Applications
{
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators;
    using ViewModels.Application.Apprenticeship;

    [TestFixture]
    public class ApprenticeshipApplicationViewModelServerValidatorTests
    {
        private ApprenticeshipApplicationViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ApprenticeshipApplicationViewModelServerValidator();
        }

        [Test]
        public void DefaultShouldNotHaveAnyValidationErrors()
        {
            var viewModel = new ApprenticeshipApplicationViewModel();

            var result = _validator.Validate(viewModel);

            result.IsValid.Should().BeTrue();
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void NotesInvalidCharacters(string notes, bool expectValid)
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Notes = notes
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Notes, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Notes, viewModel);
            }
        }
    }
}