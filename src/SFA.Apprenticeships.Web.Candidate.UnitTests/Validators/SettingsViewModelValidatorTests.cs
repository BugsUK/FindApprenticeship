namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Builders;
    using Candidate.Validators;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class SettingsViewModelValidatorTests
    {
        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(35, true)]
        [TestCase(36, false)]
        public void ShouldRequireFirstName(int? length, bool expectValid)
        {
            // Arrange.
            var firstName = length.HasValue ? new string('X', length.Value) : null;
            var viewModel = new SettingsViewModelBuilder().Firstname(firstName).Build();

            // Act.
            var validator = new SettingsViewModelClientValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(vm => vm.Firstname, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(vm => vm.Firstname, viewModel);
            }
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(33, true)]
        [TestCase(34, false)]
        public void ShouldRequireLastName(int? length, bool expectValid)
        {
            // Arrange.
            var lastName = length.HasValue ? new string('X', length.Value) : null;
            var viewModel = new SettingsViewModelBuilder().Lastname(lastName).Build();

            // Act.
            var validator = new SettingsViewModelClientValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(vm => vm.Lastname, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(vm => vm.Lastname, viewModel);
            }
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(7, false)]
        [TestCase(8, true)]
        [TestCase(16, true)]
        [TestCase(17, false)]

        public void ShouldRequirePhoneNumber(int? length, bool expectValid)
        {
            // Arrange.
            var phoneNumber = length.HasValue ? new string('0', length.Value) : null;
            var viewModel = new SettingsViewModelBuilder().PhoneNumber(phoneNumber).Build();

            // Act.
            var validator = new SettingsViewModelClientValidator();

            // Assert.
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