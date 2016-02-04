namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class BasicVacancyDetailsTests : BasicVacancyDetailsTestsBase
    {
        [Test]
        public void ShouldHaveVacancyTypeSelector()
        {
            var view = Details.RenderAsHtml(ViewModel);

            view.GetElementbyId("vacancy-type-apprenticeship").Should().NotBeNull();
            view.GetElementbyId("vacancy-type-traineeship").Should().NotBeNull();
        }

        [Test]
        public void VacancyTypeSelectorsLabel()
        {
            var view = Details.RenderAsHtml(ViewModel);

            view.GetElementbyId("vacancy-type-apprenticeship").NextSibling.InnerText.Should().Contain("Apprenticeship");
            view.GetElementbyId("vacancy-type-traineeship").NextSibling.InnerText.Should().Contain("Traineeship");
        }
    }
}