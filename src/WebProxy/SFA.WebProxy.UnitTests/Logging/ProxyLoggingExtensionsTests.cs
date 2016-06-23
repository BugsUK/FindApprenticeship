namespace SFA.WebProxy.UnitTests.Logging
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using FluentAssertions;
    using NUnit.Framework;
    using WebProxy.Logging;

    [TestFixture]
    public class ProxyLoggingExtensionsTests
    {
        private readonly string _getApprenticeshipFrameworksRequest = 
@"<soapenv:Envelope
    xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	xmlns:ns=""http://services.imservices.org.uk/AVMS/Interfaces/5.1"">
    <soapenv:Header>
        <ns:PublicKey>PublicKey</ns:PublicKey>
        <ns:MessageId>" + Guid.NewGuid() + @"</ns:MessageId>
        <ns:ExternalSystemId>${#TestSuite#ExternalSystem}</ns:ExternalSystemId>
    </soapenv:Header>
    <soapenv:Body>
        <ns:GetApprenticeshipFrameworksRequest/>
    </soapenv:Body>
</soapenv:Envelope>";

        [Test]
        public void LogRequestHeaders()
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:23791/Services/ReferenceData/ReferenceData51.svc");
            request.Headers.Add("Connection", "Keep-Alive");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Host = "localhost:23791";
            request.Headers.Add("User-Agent", "Apache-HttpClient/4.1.1 (java 1.5)");
            request.Headers.Add("SOAPAction", "\"http://services.imservices.org.uk/AVMS/Interfaces/5.1/IReferenceData/GetApprenticeshipFrameworks\"");
            var content = new StringContent(_getApprenticeshipFrameworksRequest);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml; charset=UTF-8");
            content.Headers.ContentLength = _getApprenticeshipFrameworksRequest.Length;
            request.Content = content;

            //Act
            var headersLoggingString = request.GetHeadersLoggingString();
            var headers = headersLoggingString.Split(new []{"\r\n"}, StringSplitOptions.None);

            //Assert
            headersLoggingString.Should().StartWith("Connection: Keep-Alive");
            headersLoggingString.Should().Contain("Accept-Encoding: gzip, deflate");
            headersLoggingString.Should().Contain("Host: localhost:23791");
            headersLoggingString.Should().Contain("User-Agent: Apache-HttpClient/4.1.1 (java 1.5)");
            headersLoggingString.Should().Contain("SOAPAction: \"http://services.imservices.org.uk/AVMS/Interfaces/5.1/IReferenceData/GetApprenticeshipFrameworks\"");
            headersLoggingString.Should().Contain("Content-Length: ");
            headersLoggingString.Should().Contain("Content-Type: text/xml; charset=UTF-8");
            headers.Length.Should().Be(8);
        }
    }
}