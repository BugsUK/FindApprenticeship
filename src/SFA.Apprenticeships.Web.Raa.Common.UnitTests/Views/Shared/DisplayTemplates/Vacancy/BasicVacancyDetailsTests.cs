namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using Common.Views.Shared.DisplayTemplates.Vacancy;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;
    using ViewModels.Vacancy;

    [TestFixture]
    public class BasicVacancyDetailsTests : ViewUnitTest
    {
        [Test]
        public void ShouldHaveVacancyTypeSelector()
        {
            var viewModel = new Fixture().Build<NewVacancyViewModel>().Create();
            var details = new BasicVacancyDetails();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("vacancy-type-apprenticeship").Should().NotBeNull();
            view.GetElementbyId("vacancy-type-traineeship").Should().NotBeNull();
        }

        [Test]
        public void VacancyTypeSelectorsLabel()
        {
            var viewModel = new Fixture().Build<NewVacancyViewModel>().Create();
            var details = new BasicVacancyDetails();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("vacancy-type-apprenticeship").NextSibling.InnerText.Should().Contain("Apprenticeship");
            view.GetElementbyId("vacancy-type-traineeship").NextSibling.InnerText.Should().Contain("Traineeship");
        }
    }
}