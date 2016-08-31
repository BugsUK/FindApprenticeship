namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    [Parallelizable]
    public class MonitoringInformationViewModelValidatorTests
    {
        [TestCase(0, true)]
        [TestCase(4000, true)]
        [TestCase(4001, false)]
        public void ShouldValidateSupportLength(int length, bool shouldBeValid)
        {
            var viewModel = new MonitoringInformationViewModel
            {
                AnythingWeCanDoToSupportYourInterview = new string('X', length)
            };

            var validator = new MonitoringInformationViewModelValidator();

            if (shouldBeValid)
            {
                validator.ShouldNotHaveValidationErrorFor(x => x.AnythingWeCanDoToSupportYourInterview, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(x => x.AnythingWeCanDoToSupportYourInterview, viewModel);
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldValidateNullOrWhitespaceSupportLength(string anythingWeCanDoToSupportYourInterview)
        {
            var viewModel = new MonitoringInformationViewModel
            {
                AnythingWeCanDoToSupportYourInterview = anythingWeCanDoToSupportYourInterview
            };

            var validator = new MonitoringInformationViewModelValidator();

            validator.ShouldNotHaveValidationErrorFor(x => x.AnythingWeCanDoToSupportYourInterview, viewModel);
        }

        [TestCase("<script>")]
        public void ShouldValidateWhitelistedCharacters(string anythingWeCanDoToSupportYourInterview)
        {
            var viewModel = new MonitoringInformationViewModel
            {
                AnythingWeCanDoToSupportYourInterview = anythingWeCanDoToSupportYourInterview
            };

            var validator = new MonitoringInformationViewModelValidator();

            validator.ShouldHaveValidationErrorFor(x => x.AnythingWeCanDoToSupportYourInterview, viewModel);
        }
    }
}
