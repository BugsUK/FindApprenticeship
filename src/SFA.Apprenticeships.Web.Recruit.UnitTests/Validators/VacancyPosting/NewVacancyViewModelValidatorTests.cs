namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using NUnit.Framework;
    using Recruit.Validators.VacancyPosting;
    using ViewModels.Vacancy;

    [TestFixture]
    public class NewVacancyViewModelValidatorTests
    {
        private NewVacancyViewModelValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new NewVacancyViewModelValidator();
        }

        [Test]
        [Ignore]
        public void ShouldRequireFramework()
        {
            // Arrange.
            var viewModel = new NewVacancyViewModel();

            // Act.
            _validator.Validate(viewModel);

            // Assert.
            Assert.Fail();
        }
    }
}
