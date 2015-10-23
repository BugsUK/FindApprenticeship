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
    public class EditVacancyTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new EditVacancy();

            var viewModel = new NewVacancyViewModel
            {
                SectorsAndFrameworks = new List<SectorSelectItemViewModel>(),
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel()
                {
                    Employer = new EmployerViewModel()
                    {
                        Address = new AddressViewModel()
                    }
                }
            };
           
            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("editVacancyAndExit").Should().NotBeNull("Should exists a save and exit button");
        }
    }
}
