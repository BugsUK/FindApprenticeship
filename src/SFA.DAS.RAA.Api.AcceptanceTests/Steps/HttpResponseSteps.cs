namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System;
    using System.Net;
    using Constants;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class HttpResponseSteps
    {
        [Then(@"The response status is: (.*)")]
        public void ThenTheResponseStatusIs(string statusCodeString)
        {
            var statusCode = ScenarioContext.Current.Get<HttpStatusCode>(ScenarioContextKeys.HttpResponseStatusCode);
            HttpStatusCode expectedStatusCode;
            Enum.TryParse(statusCodeString, out expectedStatusCode).Should().BeTrue();
            statusCode.Should().Be(expectedStatusCode);
        }
    }
}