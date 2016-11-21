namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.Providers
{
    using Common.UnitTests.Validators;
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Provider;
    using Raa.Common.ViewModels.Provider;
    using System;

    [TestFixture]
    [Parallelizable]
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
            var viewModel = new VacancyOwnerRelationshipViewModel
            {
                EmployerWebsiteUrl = websiteUrl,
                EmployerDescription = "populated"
            };
            string uriString = null;

            // Act.
            var validator = new VacancyOwnerRelationshipViewModelValidator();
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
        [TestCase(Samples.ValidFreeHtmlText, true)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void ShouldValidateDescription(
            string description,
            bool expectValid)
        {
            // Arrange.
            var viewModel = new VacancyOwnerRelationshipViewModel
            {
                EmployerWebsiteUrl = "http://www.valid.com",
                EmployerDescription = description,
                IsAnonymousEmployer = false
            };

            // Act.
            var validator = new VacancyOwnerRelationshipViewModelValidator();

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

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("something", true)]
        [TestCase(MassivleyLong, true)]
        [TestCase(Samples.ValidFreeHtmlText, true)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void ShouldValidateDescription_ForAnonymousEmployer(
            string description,
            bool expectValid)
        {
            // Arrange.
            var viewModel = new VacancyOwnerRelationshipViewModel
            {
                AnonymousEmployerDescription = description,
                IsAnonymousEmployer = true,
                AnonymousEmployerReason = description,
                AnonymousAboutTheEmployer = description
            };

            // Act.
            var validator = new VacancyOwnerRelationshipViewModelValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(m => m.AnonymousEmployerDescription, viewModel);
                validator.ShouldNotHaveValidationErrorFor(m => m.AnonymousAboutTheEmployer, viewModel);
                validator.ShouldNotHaveValidationErrorFor(m => m.AnonymousEmployerReason, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(m => m.AnonymousEmployerDescription, viewModel);
                validator.ShouldHaveValidationErrorFor(m => m.AnonymousAboutTheEmployer, viewModel);
                validator.ShouldHaveValidationErrorFor(m => m.AnonymousEmployerReason, viewModel);
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
