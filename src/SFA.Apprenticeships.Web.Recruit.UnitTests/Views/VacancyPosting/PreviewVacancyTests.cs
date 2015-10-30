using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy;
using SFA.Apprenticeships.Web.Recruit.Views.VacancyPosting;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using Common.ViewModels;

    [TestFixture]
    public class PreviewVacancyTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new PreviewVacancy();

            var viewModel = new VacancyViewModel() {
                ApprenticeshipLevels = new List<SelectListItem>(),
                ProviderSite = new ProviderSiteViewModel()
                {
                    Address = new AddressViewModel()
                },
                VacancySummaryViewModel = new VacancySummaryViewModel
                {
                    ClosingDate = new DateViewModel(new DateTime?()),
                    PossibleStartDate = new DateViewModel(new DateTime?())
                },
                NewVacancyViewModel = new NewVacancyViewModel
                { 
                    ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel()
                    {
                        Employer = new EmployerViewModel()
                        {
                            Address = new AddressViewModel()
                        }
                    }
                }
            };
           
            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("dashboardLink").Should().NotBeNull("Should exists a return to dashboard button");
        }
    }
}
