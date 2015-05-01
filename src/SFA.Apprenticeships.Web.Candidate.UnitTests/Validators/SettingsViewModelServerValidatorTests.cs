namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Builders;
    using Candidate.Validators;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    public class SettingsViewModelServerValidatorTests
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("0123456789", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("0123456789", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("0123456789", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("0123456789", true)]
        public void US616_AC4_PhoneNumberRequired(string phoneNumber, bool expectValid)
        {
            var viewModel = new SettingsViewModelBuilder().PhoneNumber(phoneNumber).Build();

            var validator = new SettingsViewModelServerValidator();

            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(vm => vm.PhoneNumber, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(vm => vm.PhoneNumber, viewModel);
            }
        }
    }
}