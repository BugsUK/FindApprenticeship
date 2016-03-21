namespace SFA.Apprenticeships.Web.Manage.UnitTests.Views.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Routing;
    using Common.ViewModels;
    using Common.ViewModels.Locations;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Manage.Views.Vacancy;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;
    using Raa.Common.Views.Shared.DisplayTemplates.Vacancy;
    using RazorGenerator.Testing;

    [TestFixture]
    public class ReviewVacancyTests : ViewUnitTest
    {
        HttpContextBase _context;

        [SetUp]
        public void Setup()
        {
            var request = new Mock<HttpRequestBase>();
            var routeData = new RouteData();
            routeData.Values.Add("Vacancy", "Vacancy");
            request.Setup(r => r.RequestContext).Returns(new RequestContext
            {
                RouteData = routeData
            });

            _context = new HttpContextBuilder().With(request.Object).Build();
        }

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
                    OwnerParty = new VacancyPartyViewModel()
                    {
                        Employer = new EmployerViewModel()
                        {
                            Address = new AddressViewModel()
                        }
                    },
                    OfflineVacancy = false
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel(),
                Status = VacancyStatus.ReservedForQA
            };

            var view = details.RenderAsHtml(_context, viewModel);

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
                    OwnerParty = new VacancyPartyViewModel()
                    {
                        Employer = new EmployerViewModel()
                        {
                            Address = new AddressViewModel()
                        }
                    },
                    OfflineVacancy = false
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel(),
                Status = VacancyStatus.ReservedForQA
            };

            var view = details.RenderAsHtml(_context, viewModel);

            view.GetElementbyId("btnUnReserve").Should().NotBeNull("Should exists a return to dashboard button");
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
                    OwnerParty = new VacancyPartyViewModel()
                    {
                        Employer = new EmployerViewModel()
                        {
                            Address = new AddressViewModel()
                        }
                    },
                    OfflineVacancy = false
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel(),
                Status = VacancyStatus.ReservedForQA
            };

            var view = details.RenderAsHtml(_context, viewModel);

            view.GetElementbyId("btnReject").Should().NotBeNull("Should exists a return to dashboard button");
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
                        ClosingDate = new DateViewModel(DateTime.Now),
                        PossibleStartDate = new DateViewModel(DateTime.Now)
                    },
                    WageType = wagetype,
                    HoursPerWeek = hoursPerWeek
                },
                NewVacancyViewModel = new NewVacancyViewModel
                {
                    OwnerParty = new VacancyPartyViewModel()
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
                Status = VacancyStatus.ReservedForQA,
                VacancyType = VacancyType.Apprenticeship
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
                FurtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel
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
                    OwnerParty = new VacancyPartyViewModel()
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
                Status = VacancyStatus.ReservedForQA,
                VacancyType = VacancyType.Apprenticeship
            };

            var view = details.RenderAsHtml(_context, viewModel);

            view.GetElementbyId("vacancy-wage").InnerHtml.Should().Be(expectedDisplayText);
        }
    }
}
