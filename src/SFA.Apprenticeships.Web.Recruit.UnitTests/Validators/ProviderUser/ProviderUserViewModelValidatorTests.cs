namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.ProviderUser
{
    using NUnit.Framework;
    using FluentValidation.TestHelper;
    using Raa.Common.Validators.ProviderUser;
    using Raa.Common.ViewModels.ProviderUser;

    [TestFixture]
    public class ProviderUserViewModelValidatorTests
    {
        [TestCase("", "", "")]
        [TestCase("asdf asdf asdf asdf asdf asf asdf asdf asdf asdf asdf asdf asdf asf asdf asdf asdf asdf asdf asdf asdf asf asdf asdf asdf asdf asdf asdf asdf asf asdf asdf ", "asdf", "asdf")]
        [TestCase("", "", "123654987456654234")]
        public void ShouldFailValidation(string fullName, string email, string phoneNumber)
        {
            // Arrange.
            var viewModel = new ProviderUserViewModel
            {
                Fullname = fullName,
                EmailAddress = email,
                PhoneNumber = phoneNumber
            };

            // Act.
            var validator = new ProviderUserViewModelValidator();

            // Assert.
            validator.ShouldHaveValidationErrorFor(m => m.Fullname, viewModel);
            validator.ShouldHaveValidationErrorFor(m => m.EmailAddress, viewModel);
            validator.ShouldHaveValidationErrorFor(m => m.PhoneNumber, viewModel);
        }

        [TestCase("firstname lastname", "asdf@asdf.com", "03213465454")]
        public void ShouldPassValidation(string fullName, string email, string phoneNumber)
        {
            // Arrange.
            var viewModel = new ProviderUserViewModel
            {
                Fullname = fullName,
                EmailAddress = email,
                PhoneNumber = phoneNumber
            };

            // Act.
            var validator = new ProviderUserViewModelValidator();

            // Assert.
            validator.ShouldNotHaveValidationErrorFor(m => m.Fullname, viewModel);
            validator.ShouldNotHaveValidationErrorFor(m => m.EmailAddress, viewModel);
            validator.ShouldNotHaveValidationErrorFor(m => m.PhoneNumber, viewModel);
        }
    }
}
