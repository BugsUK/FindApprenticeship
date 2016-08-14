namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Home;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    [Parallelizable]
    public class FeedbackViewModelValidatorTests
    {
        [Test]
        public void ShouldRequireDetails()
        {
            var viewModel = new FeedbackViewModel();
            var viewModelValidator = new FeedbackClientViewModelValidator();

            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Details, viewModel);
        }

        [Test]
        public void ShouldNotRequireNameAndEmail()
        {
            var viewModel = new FeedbackViewModel
            {
                Details = "Some feedback"
            };

            var viewModelValidator = new FeedbackClientViewModelValidator();

            viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Name, viewModel);
            viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Email, viewModel);
        }

        [Test]
        public void ShouldAllowOnlyWhitelistedCharacters()
        {
            var viewModel = new FeedbackViewModel
            {
                Email = "<sqlattacker>@example.com",
                Name = "<baz>",
                Details = "<script>"
            };

            var viewModelValidator = new FeedbackClientViewModelValidator();

            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Name, viewModel);
            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Email, viewModel);
            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Details, viewModel);
        }

        [Test]
        public void ShouldNotAllowNameWhenTooLong()
        {
            var viewModel = new FeedbackViewModel
            {
                Name = new string('X', 72)
            };

            var viewModelValidator = new FeedbackClientViewModelValidator();

            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Name, viewModel);
        }

        [Test]
        public void ShouldNotAllowEmailWhenTooLong()
        {
            var viewModel = new FeedbackViewModel
            {
                Email = "baz@" + new string('X', 100) + ".com"
            };

            var viewModelValidator = new FeedbackClientViewModelValidator();

            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Email, viewModel);
        }

        [Test]
        public void ShouldNotAllowDetailsWhenTooLong()
        {
            var viewModel = new FeedbackViewModel
            {
                Details= new string('X', 4001)
            };

            var viewModelValidator = new FeedbackClientViewModelValidator();

            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Details, viewModel);
        }
    }
}