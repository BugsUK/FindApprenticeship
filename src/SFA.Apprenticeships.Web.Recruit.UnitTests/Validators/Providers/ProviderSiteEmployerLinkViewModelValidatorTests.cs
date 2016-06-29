namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.Providers
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using FluentValidation.TestHelper;
    using Raa.Common.Validators.Provider;
    using Raa.Common.ViewModels.Provider;

    [TestFixture]
    public class ProviderSiteEmployerLinkViewModelValidatorTests
    {
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
        public void ShouldValidateWebSiteUrl(
            string websiteUrl,
            bool expectValid)
        {
            // Arrange.
            var viewModel = new VacancyPartyViewModel
            {
                EmployerWebsiteUrl = websiteUrl,
                EmployerDescription = "populated"
            };
            string uriString = null;

            // Act.
            var validator = new VacancyPartyViewModelValidator();
            Action uriAction = () => { uriString = new UriBuilder(viewModel.EmployerWebsiteUrl).Uri.ToString(); };

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(m => m.EmployerWebsiteUrl, viewModel);
                if (!string.IsNullOrEmpty(websiteUrl))
                {
                    uriAction.ShouldNotThrow();
                    uriString.Should().NotBeNullOrEmpty();
                }
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(m => m.EmployerWebsiteUrl, viewModel);
            }
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("something", true)]
        [TestCase(MassivleyLong, true)]
        [TestCase(Samples.ValidFreeText, true)]
        [TestCase(Samples.InvalidFreeTextWithInput, false)]
        [TestCase(Samples.InvalidFreeTextWithObject, false)]
        [TestCase(Samples.InvalidFreeTextWithScript, false)]
        public void ShouldValidateDescription(
            string description,
            bool expectValid)
        {
            // Arrange.
            var viewModel = new VacancyPartyViewModel
            {
                EmployerWebsiteUrl = "http://www.valid.com",
                EmployerDescription = description
            };

            // Act.
            var validator = new VacancyPartyViewModelValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(m => m.EmployerDescription, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(m => m.EmployerDescription, viewModel);
            }
        }

        private const string MassivleyLong = "never too long for the validator never too long for the validator " +
                                             "never too long for the validator never too long for the " +
                                             "validator never too long for the validator never too long for" +
                                             " the validator never too long for the validator never too " +
                                             "long for the validator never too long for the validator " +
                                             "never too long for the validator never too long for the " +
                                             "validator never too long for the validator never too " +
                                             "long for the validator never too long for the validator " +
                                             "never too long for the validator never too long for the " +
                                             "validator never too long for the validator never too long" +
                                             " for the validator never too long for the validator never " +
                                             "too long for the validator never too long for the validator" +
                                             " never too long for the validator never too long for the " +
                                             "validator never too long for the validator never too long for" +
                                             " the validator never too long for the validator never too " +
                                             "long for the validator never too long for the validator" +
                                             " never too long for the validator never too long for the validator never " +
                                             "too long for the validator never too long for the validator never too long for " +
                                             "the validator never too long for the validator never too long for the validator never too " +
                                             "long for the validator never too long for the validator never too long " +
                                             "for the validator never too long for the validator never too long for " +
                                             "the validator never too long for the validator never too long for the " +
                                             "validator never too long for the validator never too long for the validator never " +
                                             "too long for the validator never too long for the validator never too long for the" +
                                             " validator never too long for the validator never too long for the validator never" +
                                             " too long for the validator never too long for the validator never too long for" +
                                             " the validator never too long for the validator never too long for the validator" +
                                             " never too long for the validator never too long for the validator never too long " +
                                             "for the validator never too long for the validator never too long for the validator" +
                                             " never too long for the validator never too long for the validator never too long fo" +
                                             "r the validator never too long for the validator never too long for the validator never " +
                                             "too long for the validator never too long for the validator never too long for the" +
                                             " validator never too long for the validator never too long for the validator never " +
                                             "too long for the validator never too long for the validator never too long for the" +
                                             " validator never too long for the validator never too long for the validator never " +
                                             "too long for the validator never too long for the validator never too long for the " +
                                             "validator never too long for the validator never too long for the validator never too" +
                                             " long for the validator never too long for the validator never too long for the " +
                                             "validator never too long for the validator never too long for the validator never" +
                                             " too long for the validator never too long for the validator never too long for the" +
                                             " validator never too long for the validator never too long for the validator never " +
                                             "too long for the validator never too long for the validator never too long for the " +
                                             "validator never too long for the validator never too long for the validator never too " +
                                             "long for the validator never too long for the validator never too long for the validator " +
                                             "never too long for the validator never too long for the validator never too long for the" +
                                             " validator never too long for the validator never too long for the validator never too" +
                                             " long for the validator never too long for the validator never too long for the validator" +
                                             " never too long for the validator never too long for the validator never too long for the " +
                                             "validator never too long for the validator never too long for the validator never too" +
                                             " long for the validator never too long for the validator never too long for the validator" +
                                             " never too long for the validator never too long for the validator never too long for the" +
                                             " validator never too long for the validator never too long for the validator never too" +
                                             " long for the validator never too long for the validator never too long for the validator" +
                                             " never too long for the validator never too long for the validator never too long for the" +
                                             " validator never too long for the validator never too long for the validator never too " +
                                             "long for the validator never too long for the validator never too long for the validator " +
                                             "never too long for the validator never too long for the validator never too long for the" +
                                             " validator never too long for the validator never too long for the validator never too " +
                                             "long for the validator never too long for the validator never too long for the validator " +
                                             "never too long for the validator never too long for the validator never too long for the " +
                                             "validator never too long for the validator";
    }
}
