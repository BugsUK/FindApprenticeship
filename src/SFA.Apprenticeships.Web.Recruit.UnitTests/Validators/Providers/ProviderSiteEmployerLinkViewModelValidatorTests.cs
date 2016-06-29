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
        [TestCase(ValidFreeTextLong, true)]
        [TestCase(InvalidFreeTextLong, false)]
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

        private const string ValidFreeTextLong = "<h1>Excellent op<h2>portunities</h2> for Pre School Apprentice&#39;s are " +
                                                 "available in this established, " +
                  "friendly nursery.  You will be caring for and meeting <br>the needs of <br><br>children aged between " +
                  "4 months and 5 years, creating a safe environment and providing stimulating </h1>\r\n<br></br> <br></br>" +
                  "<br></br>";

        private const string InvalidFreeTextLong = "<h1>Excellent op<h2>portunities</h2> for Pre School Apprentice&#39;s are available in this established, \" " +
                                                   "+\r\n                  \"friendly nursery.  " +
                                                   "You will be caring for and meeting <br>the needs of <br><br>children aged between " +
                                                   "\" +\r\n                  \"4 " +
                                                   "months and 5 years, creating a safe environment and providing stimulating </h1>\\r\\n<br></br> <br>" +
                                                   "</br>\" +\r\n                  \"<br>" +
                                                   "</br><script>alert(\'Hello World\')</script>\"</br>" +
                                                   "<input>alert(\\\'Hello World\\\')</input>\"\"</br>" +
                                                   "<object>alert(\\\'Hello World\\\')</object>\"";
    }
}
