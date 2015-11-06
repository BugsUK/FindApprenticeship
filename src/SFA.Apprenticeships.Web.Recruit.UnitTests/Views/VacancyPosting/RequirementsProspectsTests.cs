using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
using SFA.Apprenticeships.Web.Recruit.Views.VacancyPosting;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    [TestFixture]
    public class RequirementsProspectsTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new VacancyRequirementsProspects();

            var viewModel = new VacancyRequirementsProspectsViewModel { };
           
            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("VacancyRequirementsProspectsAndExit").Should().NotBeNull("Should exists a save and exit button");
        }
    }
}
