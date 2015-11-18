namespace SFA.WebProxy.UnitTests
{
    using System;
    using System.Net.Http;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class NasAvWebServicesRoutingTests
    {
        [Test]
        public void RootUrl()
        {
            //Arrange
            var proxyRouting = new NasAvWebServicesRouting();
            var uri = new Uri("http://localhost");

            //Act
            var routing = proxyRouting.GetRouting(uri, HttpMethod.Get, null, null);

            //Assert
            routing.PrimaryUri.AbsoluteUri.Should().Be("https://apprenticeshipvacancymatchingservice.lsc.gov.uk/");
        }
    }
}