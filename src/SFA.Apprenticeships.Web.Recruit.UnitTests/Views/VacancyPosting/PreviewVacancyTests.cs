﻿namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using Common.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Routing;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Common.ViewModels.Locations;
    using Domain.Entities.Vacancies;
    using Moq;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;
    using Raa.Common.Views.Shared.DisplayTemplates;
    using Raa.Common.Views.Shared.DisplayTemplates.Vacancy;
    using Recruit.Views.VacancyPosting;

    [TestFixture]
    public class PreviewVacancyTests : ViewUnitTest
    {
        HttpContextBase _context;

        [SetUp]
        public void Setup()
        {
            var request = new Mock<HttpRequestBase>();
            var routeData = new RouteData();
            routeData.Values.Add("VacancyPosting", "VacancyPosting");
            request.Setup(r => r.RequestContext).Returns(new RequestContext
            {
                RouteData = routeData
            });

            _context = new HttpContextBuilder().With(request.Object).Build();
        }

        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new PreviewVacancy();

            var viewModel = new VacancyViewModel() {
                ProviderSite = new ProviderSiteViewModel()
                {
                    Address = new AddressViewModel()
                },
                VacancySummaryViewModel = new VacancySummaryViewModel
                {
                    VacancyDatesViewModel = new VacancyDatesViewModel
                    {
                        ClosingDate = new DateViewModel(DateTime.Now),
                        PossibleStartDate = new DateViewModel(DateTime.Now)
                    }
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
           
            var view = details.RenderAsHtml(_context, viewModel);

            view.GetElementbyId("dashboardLink").Should().NotBeNull("Should exists a return to dashboard button");
        }

        [TestCase(WageType.NationalMinimumWage, 30, "&#163;116.10 - &#163;201.00")]
        [TestCase(WageType.NationalMinimumWage, 37.5, "&#163;145.13 - &#163;251.25")]
        [TestCase(WageType.ApprenticeshipMinimumWage, 20, "&#163;66.00")]
        [TestCase(WageType.ApprenticeshipMinimumWage, 16, "&#163;52.80")]
        [TestCase(WageType.ApprenticeshipMinimumWage, 37.5, "&#163;123.75")]
        public void ShouldShowWageText(WageType wagetype, decimal hoursPerWeek, string expectedDisplayText)
        {
            var details = new WorkingWeekAndWage();

            var viewModel = new VacancyViewModel
            {
                ProviderSite = new ProviderSiteViewModel()
                {
                    Address = new AddressViewModel()
                },
                VacancySummaryViewModel = new VacancySummaryViewModel
                {
                    VacancyDatesViewModel = new VacancyDatesViewModel
                    {
                        ClosingDate = new DateViewModel(DateTime.Now),
                        PossibleStartDate = new DateViewModel(DateTime.Now)
                    },
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
                TrainingDetailsViewModel = new TrainingDetailsViewModel(),
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };
            
            var view = details.RenderAsHtml(_context, viewModel);

            view.GetElementbyId("vacancy-wage").InnerHtml.Should().Be(expectedDisplayText);
        }

        [TestCase(100, WageUnit.Weekly, @"&#163;100")]
        [TestCase(100, WageUnit.Monthly, @"&#163;100")]
        [TestCase(100, WageUnit.Annually, @"&#163;100")]
        public void ShouldShowCustomWageAmount(decimal wage, WageUnit wageUnit, string expectedDisplayText)
        {
            var details = new WorkingWeekAndWage();

            var viewModel = new VacancyViewModel()
            {
                ProviderSite = new ProviderSiteViewModel()
                {
                    Address = new AddressViewModel()
                },
                VacancySummaryViewModel = new VacancySummaryViewModel
                {
                    VacancyDatesViewModel = new VacancyDatesViewModel
                    {
                        ClosingDate = new DateViewModel(DateTime.Now),
                        PossibleStartDate = new DateViewModel(DateTime.Now)
                    },
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
                TrainingDetailsViewModel = new TrainingDetailsViewModel(),
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel(),
                Status = ProviderVacancyStatuses.Draft
            };

            var view = details.RenderAsHtml(_context, viewModel);

            view.GetElementbyId("vacancy-wage").InnerHtml.Should().Be(expectedDisplayText);
        }
    }
}
