namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using Common.Framework;
    using Common.ViewModels.Locations;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using Moq;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class DetailsTests : ViewUnitTest
    {
        private const string NoValue = null;
        private const string SomeString = "some string";
        private const int SomeInteger = 1;

        [Test]
        public void ShouldShowSearchReturnUrlLink()
        {
            const string someUrl = "findapprenticeship.service.gov.uk";
            var details = new Details();
            details.ViewBag.SearchReturnUrl = someUrl;

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel()
            };
            var view = details.RenderAsHtml(vacancyDetailViewModel);
            view.GetElementbyId("lnk-return-search-results").Should().NotBeNull("Return to search results should be shown.");
            view.GetElementbyId("lnk-return-search-results").Attributes["href"].Value.Should().Be(someUrl);
        }

        [Test]
        public void ShouldNotShowSearchReturnUrlLink()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel()
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);
            view.GetElementbyId("lnk-return-search-results").Should().BeNull("Return to search results should not be shown.");
            view.GetElementbyId("lnk-find-apprenticeship").Should().NotBeNull("Find an apprenticeship link should be shown.");
        }

        [Test]
        public void ShouldNotSeeAnyInfoIfModelHasError()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ViewModelMessage = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-description").Should().BeNull("Should not appear any vacancy information");
        }

        [Test]
        public void ShowDistance()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                VacancyLocationType = ApprenticeshipLocationType.NonNational,
                Distance = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-distance").InnerText.Should().Be(SomeString + " miles");
        }

        [Test]
        public void DontShowDistanceItThereIsNoDistanceToShow()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                VacancyLocationType = ApprenticeshipLocationType.NonNational
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-distance").Should().BeNull();
        }

        [Test]
        public void DontShowDistanceIfItsANationalVacancy()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                VacancyLocationType = ApprenticeshipLocationType.National
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-distance").Should().BeNull();
        }

        [Test]
        public void ShowFutureProspects()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                FutureProspects = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-future-prospects").InnerText.Should().Be(SomeString);
        }

        [Test]
        public void HideFutureProspects()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel()
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-future-prospects").Should().BeNull();
        }

        [Test]
        public void ShowRealityCheck()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                RealityCheck = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-reality-check").InnerText.Should().Be(SomeString);
        }

        [Test]
        public void HideRealityCheck()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel()
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-reality-check").Should().BeNull();
        }

        [Test]
        public void ShowEmployerDescription()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsEmployerAnonymous = false,
                EmployerDescription = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-employer-description").InnerText.Should().Be(SomeString);
        }

        [Test]
        public void HideEmployerDescription()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsEmployerAnonymous = true
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-employer-description").Should().BeNull();
        }

        [Test]
        public void ShowWellFormedEmployerWebSite()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsWellFormedEmployerWebsiteUrl = true,
                EmployerWebsite = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-employer-website").Attributes["href"].Value.Should().Be(SomeString,
                string.Format("The employer website url should be shown and should be {0}", SomeString));
        }

        [Test]
        public void ShowMalformedEmployerWebSite()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsWellFormedEmployerWebsiteUrl = false,
                EmployerWebsite = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-employer-website")
                .Should()
                .BeNull("The employer website url should not be shown");
        }

        [Test]
        public void ShowAddress()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                VacancyLocationType = ApprenticeshipLocationType.NonNational
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-address")
                .Should()
                .NotBeNull("The address should be shown");
        }

        [Test]
        public void HideAddressIfVacancyTypeIsNational()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                VacancyLocationType = ApprenticeshipLocationType.National
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-address")
                .Should()
                .BeNull("The address should not be shown if the vacancy is National");
        }

        [Test]
        public void ShowMap()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                VacancyLocationType = ApprenticeshipLocationType.NonNational
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-map")
                .Should()
                .NotBeNull("The map should be shown");
        }

        [Test]
        public void HideMapIfVacancyTypeIsNational()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                VacancyLocationType = ApprenticeshipLocationType.National
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-map")
                .Should()
                .BeNull("The map should not be shown if the vacancy is National");
        }

        [Test]
        public void ShowNasProvider()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsNasProvider = true
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-nas-provider")
                .Should()
                .NotBeNull("The nas provider message should be shown.");

            view.GetElementbyId("vacancy-framework")
                .Should()
                .BeNull("The vacancy framework should not be shown if the vacancy has nas provider");
        }

        [Test]
        public void ShowProvider()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsNasProvider = false
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-nas-provider")
                .Should()
                .BeNull("The nas provider message should not be shown.");

            view.GetElementbyId("vacancy-framework")
                .Should()
                .NotBeNull("The vacancy framework should be shown if the vacancy has a provider");
        }

        [Test]
        public void ShowTrainingToBeProvided()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsNasProvider = false,
                TrainingToBeProvided = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-framework")
                .Should()
                .NotBeNull("The vacancy framework should be shown if the vacancy has a provider");

            view.GetElementbyId("vacancy-training-to-be-provided")
                .Should()
                .NotBeNull("The vacancy training to be provided should be shown if the value is set.");
        }

        [Test]
        public void HideTrainingToBeProvided()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsNasProvider = false
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-framework")
                .Should()
                .NotBeNull("The vacancy framework should be shown if the vacancy has a provider");

            view.GetElementbyId("vacancy-training-to-be-provided")
                .Should()
                .BeNull("The vacancy training to be provided should not be shown if no value is set.");
        }

        [Test]
        public void ShowProviderSectorPassRate()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsNasProvider = false,
                ProviderSectorPassRate = SomeInteger
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-framework")
                .Should()
                .NotBeNull("The vacancy framework should be shown if the vacancy has a provider");

            view.GetElementbyId("vacancy-provider-sector-pass-rate").InnerText
                .Should().Contain(Convert.ToString(SomeInteger),
                "The vacancy provider sector pass rate should be shown if the value is set.");
        }

        [Test]
        public void ProviderSectorPassRateNotProvided()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsNasProvider = false
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-framework")
                .Should()
                .NotBeNull("The vacancy framework should be shown if the vacancy has a provider");

            view.GetElementbyId("vacancy-provider-sector-pass-rate").InnerText
                .Should()
                .Be("The training provider does not yet have a sector success rate for this type of apprenticeship training.");
        }

        [Test]
        public void HideRecruitmentAgencyIfItsAnonymous()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsRecruitmentAgencyAnonymous = true
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("recruitment-agency-name")
                .Should().BeNull("The recruitment agency should not be shown if it's anonymous.");
        }

        [Test]
        public void HideRecruitmentAgencyIfItHasNotSet()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsRecruitmentAgencyAnonymous = false,
                RecruitmentAgency = NoValue
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("recruitment-agency-name")
                .Should().BeNull("The recruitment agency should not be shown if the value is not set.");
        }

        [Test]
        public void ShowRecruitmentAgency()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsRecruitmentAgencyAnonymous = false,
                RecruitmentAgency = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("recruitment-agency-name").InnerText
                .Should()
                .Contain(SomeString, "The recruitment agency should not shown if the value is set.");
        }

        [Test]
        public void ShowProviderContact()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                Contact = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-provider-contact").InnerText
                .Should()
                .Contain(SomeString, "The provider contact should be shown if the value is set.");
        }

        [Test]
        public void HideProviderContact()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                Contact = NoValue
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-provider-contact")
                .Should()
                .BeNull("The provider contact should not be shown if the value is not set.");
        }

        [Test]
        public void ShowOtherInformation()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                OtherInformation = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("other-information").InnerText
                .Should()
                .Contain(SomeString, "Other information should be shown if the value is set.");
        }

        [Test]
        public void HideOtherInformation()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                OtherInformation = NoValue
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("other-information")
                .Should()
                .BeNull("Other information should not be shown if the value is not set.");
        }

        [Test]
        public void ShowApplyViaEmployerWebsite()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ApplyViaEmployerWebsite = true,
                ApplicationInstructions = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("application-instructions").InnerText
                .Should()
                .Be(SomeString);
        }

        [Test]
        public void HideApplyViaEmployerWebsite()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ApplyViaEmployerWebsite = false,
                ApplicationInstructions = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("application-instructions-container")
                .Should()
                .BeNull("Application instructions should not be shown if ApplyViaEmployerWebSite is set to false");
        }

        [Test]
        public void HideApplyViaEmployerWebsiteIfThereIsNotApplicationInstructions()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ApplyViaEmployerWebsite = true,
                ApplicationInstructions = NoValue
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("application-instructions-container")
                .Should()
                .BeNull("Application instructions should not be shown if there is no application instructions");
        }

        [Test]
        public void ShowBeforeApply()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ApplyViaEmployerWebsite = false
            };

            var httpContext = CreateMockContext();

            var view = details.RenderAsHtml(httpContext, vacancyDetailViewModel);

            view.GetElementbyId("before-apply")
                .Should()
                .NotBeNull("Before apply should be shown if the user is authenticated and she has to apply via our website.");
        }

        [Test]
        public void HideShowBeforeApplyIfApplyingViaEmployerWebsite()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ApplyViaEmployerWebsite = true
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("before-apply")
                .Should()
                .BeNull("Before apply should not be shown if the user has to apply via employer's website");
        }

        [Test]
        public void HideShowBeforeApplyIfTheUserIsAuthenticated()
        {
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ApplyViaEmployerWebsite = false
            };

            var httpContext = CreateMockContext(true);

            var view = details.RenderAsHtml(httpContext, vacancyDetailViewModel);

            view.GetElementbyId("before-apply")
                .Should()
                .BeNull("Before apply should not be shown if the user is authenticated");
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(7)]
        [TestCase(42)]
        public void ShowPostedDate(int daysAgo)
        {
            // Arrange.
            var postedDate = DateTime.Today.AddDays(-daysAgo);
            var friendlyPostedDate = postedDate.ToFriendlyDaysAgo();

            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                PostedDate = postedDate
            };

            var httpContext = CreateMockContext();

            // Act.
            var view = details.RenderAsHtml(httpContext, vacancyDetailViewModel);

            // Assert.
            var element = view.GetElementbyId("vacancy-posted-date");

            element.Should().NotBeNull();
            element.InnerText.Should().Be(friendlyPostedDate);
        }

        [TestCase(5)]
        [TestCase(1)]
        public void ShowNumberOfPositions(int numberOfPositions)
        {
            // Arrange.
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                NumberOfPositions = numberOfPositions
            };

            // Act.
            var view = details.RenderAsHtml(vacancyDetailViewModel);

            // Assert.
            var element = view.GetElementbyId("number-of-positions");

            element.Should().NotBeNull();
            element.InnerText.Should().Contain(numberOfPositions.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase(null)]
        [TestCase(false)]
        [TestCase(true)]
        public void ShowHideSaveVacancyLink(bool? isCandidateActivated)
        {
            // Arrange.
            var details = new Details();

            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
            };

            details.ViewBag.IsCandidateActivated = isCandidateActivated;

            // Act.
            var view = details.RenderAsHtml(vacancyDetailViewModel);

            // Assert.
            var element = view.GetElementbyId("save-vacancy-link");

            if (isCandidateActivated.HasValue && isCandidateActivated.Value)
            {
                element.Should().NotBeNull();
            }
            else
            {
                element.Should().BeNull();
            }
        }
        private static HttpContextBase CreateMockContext(bool isAuthenticated = false)
        {
            // Use Moq for faking context objects as it can setup all members
            // so that by default, calls to the members return a default/null value 
            // instead of a not implemented exception.

            // members were we want specific values returns are setup explicitly.

            // mock the request object
            var mockRequest = new Mock<HttpRequestBase>(MockBehavior.Loose);
            mockRequest.Setup(m => m.IsLocal).Returns(false);
            mockRequest.Setup(m => m.ApplicationPath).Returns("/");
            mockRequest.Setup(m => m.ServerVariables).Returns(new NameValueCollection());
            mockRequest.Setup(m => m.RawUrl).Returns(string.Empty);
            mockRequest.Setup(m => m.Cookies).Returns(new HttpCookieCollection());
            mockRequest.Setup(m => m.IsAuthenticated).Returns(isAuthenticated);

            // mock the response object
            var mockResponse = new Mock<HttpResponseBase>(MockBehavior.Loose);
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(virtualPath => virtualPath);
            mockResponse.Setup(m => m.Cookies).Returns(new HttpCookieCollection());

            // mock the httpcontext

            var mockHttpContext = new Mock<HttpContextBase>(MockBehavior.Loose);
            mockHttpContext.Setup(m => m.Items).Returns(new Hashtable());
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockHttpContext.Object;
        }
    }
}