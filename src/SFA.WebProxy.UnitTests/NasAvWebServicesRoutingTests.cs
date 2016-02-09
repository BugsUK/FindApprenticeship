namespace SFA.WebProxy.UnitTests
{
    using System;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using Configuration;
    using FluentAssertions;
    using Models;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Routing;

    [TestFixture]
    public class NasAvWebServicesRoutingTests
    {
        private const string NasAvWebServiceRootUriString = "https://apprenticeshipvacancymatchingservice.lsc.gov.uk";
        private const string CompatabilityWebServiceRootUriString = "http://sfa-service-sit.cloudapp.net";

        private static readonly Guid RoutedExternalSystemId = Guid.NewGuid();
        private static readonly Guid NonRoutedExternalSystemId = Guid.NewGuid();

        private Mock<IConfiguration> _configuration;
        private Mock<IWebProxyUserRepository> _webProxyUserRepository;
        private NasAvWebServicesRouting _proxyRouting;

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(c => c.NasAvWebServiceRootUri).Returns(new Uri(NasAvWebServiceRootUriString));
            _configuration.Setup(c => c.CompatabilityWebServiceRootUri).Returns(new Uri(CompatabilityWebServiceRootUriString));
            _configuration.Setup(c => c.AutomaticRouteToCompatabilityWebServiceRegex).Returns(new Regex(@"^.+?\/(ReferenceData51\.svc|VacancyDetails51\.svc|VacancySummary51\.svc)$", RegexOptions.IgnoreCase));

            _webProxyUserRepository = new Mock<IWebProxyUserRepository>();
            _webProxyUserRepository.Setup(r => r.Get(RoutedExternalSystemId))
                .Returns(new WebProxyConsumer
                {
                    WebProxyConsumerId = 1,
                    ExternalSystemId = RoutedExternalSystemId,
                    RouteToCompatabilityWebServiceRegex =
                        @"^.+?\/(ApplicationTracking51\.svc|VacancyManagement51\.svc)$"
                });

            _proxyRouting = new NasAvWebServicesRouting(_configuration.Object, _webProxyUserRepository.Object);
        }

        [Test]
        public void RootUrl()
        {
            //Arrange
            var uri = new Uri("http://localhost:23791");

            //Act
            var routing = _proxyRouting.GetRouting(uri, HttpMethod.Get, null, null, new RouteIdentifier());

            //Assert
            routing.Routes.Count.Should().Be(2);
            routing.Routes[0].Uri.AbsoluteUri.Should().Be(NasAvWebServiceRootUriString + "/");
            routing.Routes[1].Uri.AbsoluteUri.Should().Be(CompatabilityWebServiceRootUriString + "/");
        }

        [Test]
        public void LandingPage()
        {
            //Arrange
            var uri = new Uri("http://localhost:23791/navms/Forms/Candidate/VisitorLanding.aspx");

            //Act
            var routing = _proxyRouting.GetRouting(uri, HttpMethod.Get, null, null, new RouteIdentifier());

            //Assert
            routing.Routes.Count.Should().Be(2);
            routing.Routes[0].Uri.AbsoluteUri.Should().Be(NasAvWebServiceRootUriString + "/navms/Forms/Candidate/VisitorLanding.aspx");
            routing.Routes[1].Uri.AbsoluteUri.Should().Be(CompatabilityWebServiceRootUriString + "/navms/Forms/Candidate/VisitorLanding.aspx");
        }

        [Test]
        public void ReferenceDataPage()
        {
            //Arrange
            var uri = new Uri("http://localhost:23791/Services/ReferenceData/ReferenceData51.svc");

            //Act
            var routing = _proxyRouting.GetRouting(uri, HttpMethod.Get, null, null, new RouteIdentifier());

            //Assert
            routing.Routes.Count.Should().Be(2);
            routing.Routes[0].Uri.AbsoluteUri.Should().Be(NasAvWebServiceRootUriString + "/Services/ReferenceData/ReferenceData51.svc");
            routing.Routes[1].Uri.AbsoluteUri.Should().Be(CompatabilityWebServiceRootUriString + "/ReferenceData51.svc");
        }

        [Test]
        public void VacancyDetailsPage()
        {
            //Arrange
            var uri = new Uri("http://localhost:23791/Services/VacancyDetails/VacancyDetails51.svc");

            //Act
            var routing = _proxyRouting.GetRouting(uri, HttpMethod.Get, null, null, new RouteIdentifier());

            //Assert
            routing.Routes.Count.Should().Be(2);
            routing.Routes[0].Uri.AbsoluteUri.Should().Be(NasAvWebServiceRootUriString + "/Services/VacancyDetails/VacancyDetails51.svc");
            routing.Routes[1].Uri.AbsoluteUri.Should().Be(CompatabilityWebServiceRootUriString + "/VacancyDetails51.svc");
        }

        [Test]
        public void ReferenceDataPage_LiveServiceOnly()
        {
            //Arrange
            _configuration.Setup(c => c.NasAvWebServiceRootUri).Returns(new Uri(NasAvWebServiceRootUriString + "/Services"));
            var uri = new Uri("http://localhost:23791/ReferenceData/ReferenceData51.svc");

            //Act
            var routing = _proxyRouting.GetRouting(uri, HttpMethod.Get, null, null, new RouteIdentifier());

            //Assert
            routing.Routes.Count.Should().Be(2);
            routing.Routes[0].Uri.AbsoluteUri.Should().Be(NasAvWebServiceRootUriString + "/Services/ReferenceData/ReferenceData51.svc");
            routing.Routes[1].Uri.AbsoluteUri.Should().Be(CompatabilityWebServiceRootUriString + "/ReferenceData51.svc");
        }

        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Sandbox/ApplicationTracking/ApplicationTracking51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/ApplicationTracking/ApplicationTracking51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/ReferenceData/ReferenceData51.svc", true)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/VacancyDetails/VacancyDetails51.svc", true)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Sandbox/VacancyManagement/VacancyManagement51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/services/VacancyManagement/VacancyManagement51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/VacancySummary/VacancySummary51.svc", true)]
        public void IsAutomaticRouteToCompatabilityWebServiceUri(string uriString, bool expectTrue)
        {
            //Arrange
            var uri = new Uri(uriString);

            //Act
            var isAutomaticRouteToCompatabilityWebServiceUri = _proxyRouting.IsAutomaticRouteToCompatabilityWebServiceUri(uri);

            //Assert
            isAutomaticRouteToCompatabilityWebServiceUri.Should().Be(expectTrue);
        }

        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Sandbox/ApplicationTracking/ApplicationTracking51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/ApplicationTracking/ApplicationTracking51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/ReferenceData/ReferenceData51.svc", true)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/VacancyDetails/VacancyDetails51.svc", true)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Sandbox/VacancyManagement/VacancyManagement51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/services/VacancyManagement/VacancyManagement51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/VacancySummary/VacancySummary51.svc", true)]
        public void RouteToCompatabilityWebServiceUri(string uriString, bool compatabilityWebServiceShouldBePrimary)
        {
            //Arrange
            var uri = new Uri(uriString);

            //Act
            var routing = _proxyRouting.GetRouting(uri, HttpMethod.Get, null, null, new RouteIdentifier());

            //Assert
            routing.Routes.Count.Should().Be(2);
            routing.Routes[0].Uri.AbsoluteUri.Should().StartWith(NasAvWebServiceRootUriString);
            routing.Routes[0].IsPrimary.Should().Be(!compatabilityWebServiceShouldBePrimary);
            routing.Routes[1].Uri.AbsoluteUri.Should().StartWith(CompatabilityWebServiceRootUriString);
            routing.Routes[1].IsPrimary.Should().Be(compatabilityWebServiceShouldBePrimary);
        }

        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Sandbox/ApplicationTracking/ApplicationTracking51.svc", true)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/ApplicationTracking/ApplicationTracking51.svc", true)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/ReferenceData/ReferenceData51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/VacancyDetails/VacancyDetails51.svc", false)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Sandbox/VacancyManagement/VacancyManagement51.svc", true)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/services/VacancyManagement/VacancyManagement51.svc", true)]
        [TestCase("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/VacancySummary/VacancySummary51.svc", false)]
        public void RouteToCompatabilityWebServiceUri_WebProxyUser(string uriString, bool compatabilityWebServiceShouldBePrimary)
        {
            //Arrange
            _configuration.Setup(c => c.AutomaticRouteToCompatabilityWebServiceRegex).Returns(new Regex(""));
            _proxyRouting = new NasAvWebServicesRouting(_configuration.Object, _webProxyUserRepository.Object);
            var uri = new Uri(uriString);

            //Act
            var routing = _proxyRouting.GetRouting(uri, HttpMethod.Get, null, GetRequestContent(RoutedExternalSystemId), new RouteIdentifier());

            //Assert
            routing.Routes.Count.Should().Be(2);
            routing.Routes[0].Uri.AbsoluteUri.Should().StartWith(NasAvWebServiceRootUriString);
            routing.Routes[0].IsPrimary.Should().Be(!compatabilityWebServiceShouldBePrimary);
            routing.Routes[1].Uri.AbsoluteUri.Should().StartWith(CompatabilityWebServiceRootUriString);
            routing.Routes[1].IsPrimary.Should().Be(compatabilityWebServiceShouldBePrimary);
        }

        private string GetRequestContent(Guid externalSystemId)
        {
            return @"<soapenv:Envelope
	xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	xmlns:ns=""http://services.imservices.org.uk/AVMS/Interfaces/5.1"">
   <soapenv:Header>
      <ns:ExternalSystemId>" + externalSystemId + @"</ns:ExternalSystemId>
      <ns:MessageId>" + Guid.NewGuid() + @"</ns:MessageId>
      <ns:PublicKey>sjhadgklhsadkgjhskjl</ns:PublicKey>
   </soapenv:Header>
   <soapenv:Body>
      <ns:GetApprenticeshipFrameworksRequest/>
   </soapenv:Body>
</soapenv:Envelope>";
        }
    }
}