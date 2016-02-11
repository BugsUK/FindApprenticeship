namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Views.VacancyPosting;

    [TestFixture]
    public class VacancySubmittedTests : ViewUnitTest
    {
        [Test]
        public void ShouldHaveVacancyTypeSelector()
        {
            //Arrange
            var viewModel = new Fixture().Build<SubmittedVacancyViewModel>().Create();
            var details = new VacancySubmitted();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
        }
    }
}