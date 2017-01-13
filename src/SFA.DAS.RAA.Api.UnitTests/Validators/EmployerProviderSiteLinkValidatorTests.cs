namespace SFA.DAS.RAA.Api.UnitTests.Validators
{
    using Api.Validators;
    using FluentValidation.TestHelper;
    using Models;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class EmployerProviderSiteLinkValidatorTests
    {
        [Test]
        public void EmployerIdentifierIsRequired()
        {
            var employerProviderSiteLink = new EmployerProviderSiteLinkRequest
            {
                EmployerEdsUrn = null
            };

            var validator = new EmployerProviderSiteLinkRequestValidator();

            validator.ShouldHaveValidationErrorFor(el => el.EmployerEdsUrn, employerProviderSiteLink).WithErrorMessage("You must specify the employer's EDSURN.");
        }

        [Test]
        public void ProviderSiteIdentifierIsRequired()
        {
            var employerProviderSiteLink = new EmployerProviderSiteLinkRequest
            {
                ProviderSiteEdsUrn = null
            };

            var validator = new EmployerProviderSiteLinkRequestValidator();

            validator.ShouldHaveValidationErrorFor(el => el.ProviderSiteEdsUrn, employerProviderSiteLink).WithErrorMessage("You must specify the provider site's EDSURN.");
        }

        [Test]
        public void EmployerDescriptionIsRequired()
        {
            var employerProviderSiteLink = new EmployerProviderSiteLinkRequest
            {
                EmployerDescription = null
            };

            var validator = new EmployerProviderSiteLinkRequestValidator();

            validator.ShouldHaveValidationErrorFor(el => el.EmployerDescription, employerProviderSiteLink).WithErrorMessage("Please supply a description for the employer.");
        }

        [TestCase("Employer description", true, null)]
        [TestCase("<p>Employer description</p>", true, null)]
        [TestCase("|Employer description|", false, "The description for the employer contains some invalid characters.")]
        [TestCase("<script>Employer description</script>", false, "The description for the employer contains some invalid tags.")]
        [TestCase("<input>Employer description</input>", false, "The description for the employer contains some invalid tags.")]
        [TestCase("<object>Employer description</object>", false, "The description for the employer contains some invalid tags.")]
        public void EmployerDescriptionInputValidation(string employerDescription, bool expectValid, string expectedErrorMessage)
        {
            var employerProviderSiteLink = new EmployerProviderSiteLinkRequest
            {
                EmployerDescription = employerDescription
            };

            var validator = new EmployerProviderSiteLinkRequestValidator();

            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(el => el.EmployerDescription, employerProviderSiteLink);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(el => el.EmployerDescription, employerProviderSiteLink).WithErrorMessage(expectedErrorMessage);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("www.google.com", true)]
        [TestCase("http://www.google.com", true)]
        [TestCase("https://www.google.com", true)]
        [TestCase("www\\asdf\\com", false)]
        [TestCase("cantbemissingdot", false)]
        [TestCase("canbeanythingwithcorrect.chars", true)]
        [TestCase("cantbeincorrechars@%", false)]
        [TestCase("www.me-you.com", true)]
        public void ShouldValidateWebSiteUrl(string websiteUrl, bool expectValid)
        {
            var employerProviderSiteLink = new EmployerProviderSiteLinkRequest
            {
                EmployerWebsiteUrl = websiteUrl
            };

            var validator = new EmployerProviderSiteLinkRequestValidator();

            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(el => el.EmployerWebsiteUrl, employerProviderSiteLink);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(el => el.EmployerWebsiteUrl, employerProviderSiteLink).WithErrorMessage("Please supply a valid website url for the employer.");
            }
        }
    }
}