namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    [Parallelizable]
    public class AboutYouViewModelValidatorTests
    {
        [Test]
        public void ShouldNotHaveErrorsOnSaveWhenBlank()
        {
            var viewModel = new AboutYouViewModel();
            var viewModelSaveValidator = new AboutYouViewModelSaveValidator();

            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.WhatAreYourStrengths, viewModel);
            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.WhatDoYouFeelYouCouldImprove, viewModel);
            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.WhatAreYourHobbiesInterests, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnServerWhenBlank()
        {
            var viewModel = new AboutYouViewModel();
            var viewModelServerValidator = new AboutYouViewModelServerValidator();

            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.WhatAreYourStrengths, viewModel);
            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.WhatDoYouFeelYouCouldImprove, viewModel);
            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.WhatAreYourHobbiesInterests, viewModel);
        }
    }
}
