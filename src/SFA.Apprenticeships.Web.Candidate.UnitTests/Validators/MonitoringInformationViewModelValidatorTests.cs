namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;
    using NUnit.Framework.Constraints;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class MonitoringInformationViewModelValidatorTests
    {
        [TestCase(true, null)]
        [TestCase(true, "")]
        [TestCase(true, " ")]
        public void ShouldRequireInterviewSupportText(bool requiresSupport, string support)
        {
            var viewModel = new MonitoringInformationViewModel
            {
                RequiresSupportForInterview = requiresSupport,
                AnythingWeCanDoToSupportYourInterview = support
            };

            var validator = new MonitoringInformationViewModelValidator();

            validator.ShouldHaveValidationErrorFor(x => x.AnythingWeCanDoToSupportYourInterview, viewModel);
        }

        [TestCase(true, "Braille")]
        [TestCase(false, "Interpreter")]
        public void ShouldValidateInterviewSupportText(bool requiresSupport, string support)
        {
            var viewModel = new MonitoringInformationViewModel
            {
                RequiresSupportForInterview = requiresSupport,
                AnythingWeCanDoToSupportYourInterview = support
            };

            var validator = new MonitoringInformationViewModelValidator();

            validator.ShouldNotHaveValidationErrorFor(x => x.AnythingWeCanDoToSupportYourInterview, viewModel);
        }
    }
}
