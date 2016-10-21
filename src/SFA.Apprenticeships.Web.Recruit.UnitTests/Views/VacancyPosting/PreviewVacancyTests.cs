namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using Common.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Routing;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Common.ViewModels.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Moq;
    using Raa.Common.ViewModels.Employer;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;
    using Raa.Common.Views.Shared.DisplayTemplates.Vacancy;
    using Recruit.Views.VacancyPosting;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

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
                FurtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel
                {
                    VacancyDatesViewModel = new VacancyDatesViewModel
                    {
                        ClosingDate = new DateViewModel(DateTime.Now),
                        PossibleStartDate = new DateViewModel(DateTime.Now)
                    }
                },
                NewVacancyViewModel = new NewVacancyViewModel
                { 
                    VacancyOwnerRelationship = new VacancyOwnerRelationshipViewModel()
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

        [TestCase(WageType.NationalMinimum, 30, "&#163;116.10 - &#163;201.00")]
        [TestCase(WageType.NationalMinimum, 37.5, "&#163;145.13 - &#163;251.25")]
        [TestCase(WageType.ApprenticeshipMinimum, 20, "&#163;66.00")]
        [TestCase(WageType.ApprenticeshipMinimum, 16, "&#163;52.80")]
        [TestCase(WageType.ApprenticeshipMinimum, 37.5, "&#163;123.75")]
        public void ShouldShowWageText(WageType wagetype, decimal hoursPerWeek, string expectedDisplayText)
        {
            var details = new WorkingWeekAndWage();

            var viewModel = new VacancyViewModel
            {
                ProviderSite = new ProviderSiteViewModel()
                {
                    Address = new AddressViewModel()
                },
                FurtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel
                {
                    VacancyDatesViewModel = new VacancyDatesViewModel
                    {
                        ClosingDate = new DateViewModel(new DateTime(2016, 09, 01)),
                        PossibleStartDate = new DateViewModel(new DateTime(2016, 09, 08))
                    },
                    Wage = new WageViewModel() { Type = wagetype, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.NotApplicable, HoursPerWeek = hoursPerWeek }
                },
                NewVacancyViewModel = new NewVacancyViewModel
                {
                    VacancyOwnerRelationship = new VacancyOwnerRelationshipViewModel()
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
                VacancyType = VacancyType.Apprenticeship
            };
            
            var view = details.RenderAsHtml(_context, viewModel);

            view.GetElementbyId("vacancy-wage").InnerHtml.Should().Be(expectedDisplayText);
        }

        [TestCase(100, WageUnit.Weekly, @"&#163;100.00")]
        [TestCase(100, WageUnit.Monthly, @"&#163;100.00")]
        [TestCase(100, WageUnit.Annually, @"&#163;100.00")]
        public void ShouldShowCustomWageAmount(decimal wage, WageUnit wageUnit, string expectedDisplayText)
        {
            var details = new WorkingWeekAndWage();

            var viewModel = new VacancyViewModel()
            {
                ProviderSite = new ProviderSiteViewModel()
                {
                    Address = new AddressViewModel()
                },
                FurtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel
                {
                    VacancyDatesViewModel = new VacancyDatesViewModel
                    {
                        ClosingDate = new DateViewModel(DateTime.Now),
                        PossibleStartDate = new DateViewModel(DateTime.Now)
                    },
                    Wage = new WageViewModel() { Type = WageType.Custom, Amount = wage, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = wageUnit, HoursPerWeek = null }
                },
                NewVacancyViewModel = new NewVacancyViewModel
                {
                    VacancyOwnerRelationship = new VacancyOwnerRelationshipViewModel()
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
                Status = VacancyStatus.Draft,
                VacancyType = VacancyType.Apprenticeship
            };

            var view = details.RenderAsHtml(_context, viewModel);

            view.GetElementbyId("vacancy-wage").InnerHtml.Should().Be(expectedDisplayText);
        }
    }
}
