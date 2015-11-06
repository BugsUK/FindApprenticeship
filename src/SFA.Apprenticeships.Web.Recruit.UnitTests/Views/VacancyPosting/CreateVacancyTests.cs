using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy;
using SFA.Apprenticeships.Web.Recruit.Views.VacancyPosting;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using System.Web.Mvc;

    [TestFixture]
    public class CreateVacancyTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new CreateVacancy();

            var viewModel = new NewVacancyViewModel
            {
                SectorsAndFrameworks = new List<SelectListItem>(),
                Standards = new List<StandardViewModel>(),
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel()
                {
                    Employer = new EmployerViewModel()
                    {
                        Address = new AddressViewModel()
                    }
                }
            };
           
            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("createVacancyAndExit").Should().NotBeNull("Should exists a save and exit button");
        }
    }
}
