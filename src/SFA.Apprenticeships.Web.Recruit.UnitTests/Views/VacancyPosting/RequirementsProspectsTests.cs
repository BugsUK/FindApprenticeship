using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Frameworks;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy;
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
