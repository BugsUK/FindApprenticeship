namespace SFA.WebProxy.UnitTests
{
    using System;
    using System.Net.Http;
    using Configuration;
    using FluentAssertions;
    using Models;
    using Moq;
    using NUnit.Framework;
    using Routing;

    [TestFixture]
    public class NasAvWebServicesRoutingTests
    {
        private IProxyRouting _proxyRouting;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var config = new Mock<IConfiguration>();
            config.Setup(c => c.NasAvWebServiceRootUri).Returns("https://apprenticeshipvacancymatchingservice.lsc.gov.uk");
            config.Setup(c => c.CompatabilityWebServiceRootUrl).Returns("http://sfa-service-sit.cloudapp.net");

            _proxyRouting = new NasAvWebServicesRouting(config.Object);
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
            routing.Routes[0].Uri.AbsoluteUri.Should().Be("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/");
            routing.Routes[1].Uri.AbsoluteUri.Should().Be("http://sfa-service-sit.cloudapp.net/");
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
            routing.Routes[0].Uri.AbsoluteUri.Should().Be("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/navms/Forms/Candidate/VisitorLanding.aspx");
            routing.Routes[1].Uri.AbsoluteUri.Should().Be("http://sfa-service-sit.cloudapp.net/navms/Forms/Candidate/VisitorLanding.aspx");
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
            routing.Routes[0].Uri.AbsoluteUri.Should().Be("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/ReferenceData/ReferenceData51.svc");
            routing.Routes[1].Uri.AbsoluteUri.Should().Be("http://sfa-service-sit.cloudapp.net/ReferenceData51.svc");
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
            routing.Routes[0].Uri.AbsoluteUri.Should().Be("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/Services/VacancyDetails/VacancyDetails51.svc");
            routing.Routes[1].Uri.AbsoluteUri.Should().Be("http://sfa-service-sit.cloudapp.net/VacancyDetails51.svc");
        }
    }
}