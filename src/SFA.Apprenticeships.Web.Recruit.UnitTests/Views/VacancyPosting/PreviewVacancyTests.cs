namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using Common.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Common.ViewModels.Locations;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using Recruit.Views.VacancyPosting;

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
                    ClosingDate = new DateViewModel(DateTime.Now),
                    PossibleStartDate = new DateViewModel(DateTime.Now)
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
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };
           
            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("dashboardLink").Should().NotBeNull("Should exists a return to dashboard button");
        }

        [TestCase(WageType.NationalMinimumWage, "National Minimum Wage")]
        [TestCase(WageType.ApprenticeshipMinimumWage, "National Minimum Wage for apprentices")]
        public void ShouldShowWageText(WageType wagetype, string expectedDisplayText)
        {
            var details = new PreviewVacancy();

            var viewModel = new VacancyViewModel()
            {
                ApprenticeshipLevels = new List<SelectListItem>(),
                ProviderSite = new ProviderSiteViewModel()
                {
                    Address = new AddressViewModel()
                },
                VacancySummaryViewModel = new VacancySummaryViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.Now),
                    PossibleStartDate = new DateViewModel(DateTime.Now),
                    WageType = wagetype
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
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("vacancy-wage").InnerHtml.Should().Be(expectedDisplayText);
        }

        [TestCase(100, WageUnit.Weekly, @"&#163;100")]
        [TestCase(100, WageUnit.Monthly, @"&#163;100")]
        [TestCase(100, WageUnit.Annually, @"&#163;100")]
        public void ShouldShowCustomWageAmount(decimal wage, WageUnit wageUnit, string expectedDisplayText)
        {
            var details = new PreviewVacancy();

            var viewModel = new VacancyViewModel()
            {
                ApprenticeshipLevels = new List<SelectListItem>(),
                ProviderSite = new ProviderSiteViewModel()
                {
                    Address = new AddressViewModel()
                },
                VacancySummaryViewModel = new VacancySummaryViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.Now),
                    PossibleStartDate = new DateViewModel(DateTime.Now),
                    WageType = WageType.Custom,
                    WageUnit = wageUnit,
                    Wage = wage
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
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("vacancy-wage").InnerHtml.Should().Be(expectedDisplayText);
        }
    }
}
