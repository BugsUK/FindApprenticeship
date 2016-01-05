using ReflectionMagic;

namespace SFA.Apprenticeships.Web.Manage.UnitTests.Views.VacancyPosting
{
    using System;
    using NUnit.Framework;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Common.ViewModels;
    using Common.ViewModels.Locations;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using FluentAssertions;
    using RazorGenerator.Testing;
    using Manage.Views.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;
    using Raa.Common.Views.Shared.DisplayTemplates;

    [TestFixture]
    public class ReviewVacancyTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowApproveButton()
        {
            var details = new Review();

            var viewModel = new VacancyViewModel()
            {
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
                    },
                    OfflineVacancy = false
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("btnApprove").Should().NotBeNull("Should exists an Approve button");
        }

        [Test]
        public void ShouldShowDashboardButton()
        {
            var details = new Review();

            var viewModel = new VacancyViewModel()
            {
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
                    },
                    OfflineVacancy = false
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("dashboardLink").Should().NotBeNull("Should exists a return to dashboard button");
        }

        [Test]
        public void ShouldShowRejectButton()
        {
            var details = new Review();

            var viewModel = new VacancyViewModel()
            {
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
                    },
                    OfflineVacancy = false
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("btnReject").Should().NotBeNull("Should exists a return to dashboard button");
        }

        [TestCase(WageType.NationalMinimumWage, 30, "&#163;116.10 - &#163;201.00")]
        [TestCase(WageType.NationalMinimumWage, 37.5, "&#163;145.13 - &#163;251.25")]
        [TestCase(WageType.ApprenticeshipMinimumWage, 20, "&#163;66.00")]
        [TestCase(WageType.ApprenticeshipMinimumWage, 16, "&#163;52.80")]
        [TestCase(WageType.ApprenticeshipMinimumWage, 37.5, "&#163;123.75")]
        public void ShouldShowWageText(WageType wagetype, decimal hoursPerWeek, string expectedDisplayText)
        {
            var details = new VacancyPreview_();

            var viewModel = new VacancyViewModel
            {
                ProviderSite = new ProviderSiteViewModel()
                {
                    Address = new AddressViewModel()
                },
                VacancySummaryViewModel = new VacancySummaryViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.Now),
                    PossibleStartDate = new DateViewModel(DateTime.Now),
                    WageType = wagetype,
                    HoursPerWeek = hoursPerWeek
                },
                NewVacancyViewModel = new NewVacancyViewModel
                {
                    ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel()
                    {
                        Employer = new EmployerViewModel()
                        {
                            Address = new AddressViewModel()
                        }
                    },
                    LocationAddresses = new List<VacancyLocationAddressViewModel>(),
                    OfflineVacancy = false
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
            var details = new VacancyPreview_();

            var viewModel = new VacancyViewModel()
            {
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
                    },
                    LocationAddresses = new List<VacancyLocationAddressViewModel>(),
                    OfflineVacancy = false
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("vacancy-wage").InnerHtml.Should().Be(expectedDisplayText);
        }
    }
}
