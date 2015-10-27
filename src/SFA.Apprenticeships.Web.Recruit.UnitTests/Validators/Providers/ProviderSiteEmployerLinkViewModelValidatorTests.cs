namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.Providers
{
    using NUnit.Framework;
    using Recruit.Validators.Provider;
    using FluentValidation.TestHelper;
    using ViewModels.Provider;

    [TestFixture]
    public class ProviderSiteEmployerLinkViewModelValidatorTests
    {
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("www.google.com", true)]
        [TestCase("http://www.google.com", true)]
        [TestCase("https://www.google.com", true)]
        [TestCase("www\\asdf\\com", false)]
        [TestCase("canbeanythingwithcorrechars", true)]
        [TestCase("cantbeincorrechars@%", false)]
        public void ShouldValidateWebSiteUrl(
            string websiteUrl,
            bool expectValid)
        {
            // Arrange.
            var viewModel = new ProviderSiteEmployerLinkViewModel
            {
                WebsiteUrl = websiteUrl,
                Description = "populated"
            };

            // Act.
            var validator = new ProviderSiteEmployerLinkViewModelValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(m => m.WebsiteUrl, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(m => m.WebsiteUrl, viewModel);
            }
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("something", true)]
        [TestCase(MassivleyLong, true)]
        public void ShouldValidateDescription(
            string description,
            bool expectValid)
        {
            // Arrange.
            var viewModel = new ProviderSiteEmployerLinkViewModel
            {
                WebsiteUrl = "http://www.valid.com",
                Description = description
            };

            // Act.
            var validator = new ProviderSiteEmployerLinkViewModelValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(m => m.Description, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(m => m.Description, viewModel);
            }
        }

        private const string MassivleyLong = "never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator never too long for the validator";
    }
}
