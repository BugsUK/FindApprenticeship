using SFA.Apprenticeships.Web.Raa.Common.Validators.Provider;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.Providers
{
    using NUnit.Framework;
    using FluentValidation.TestHelper;

    [TestFixture]
    public class ProviderSiteSearchViewModelValidatorTests
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("ABC", false)]
        [TestCase("9 9", false)]
        [TestCase("9", true)]
        [TestCase("99", true)]
        [TestCase(" 99 ", true)]
        public void ShouldRequireNumericEmployerReferenceNumber(
            string employerReferenceNumber,
            bool expectValid)
        {
            // Arrange.
            var viewModel = new ProviderSiteSearchViewModel
            {
                SiteSearchMode = ProviderSiteSearchMode.EmployerReferenceNumber,
                EmployerReferenceNumber = employerReferenceNumber
            };

            // Act.
            var validator = new ProviderSiteSearchViewModelValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(m => m.EmployerReferenceNumber, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(m => m.EmployerReferenceNumber, viewModel);
            }
        }
    }
}
