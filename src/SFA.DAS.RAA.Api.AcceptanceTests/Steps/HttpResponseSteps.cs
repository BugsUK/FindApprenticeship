namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System;
    using System.Net;
    using Constants;
    using FluentAssertions;
    using Models;
    using TechTalk.SpecFlow;

    [Binding]
    public class HttpResponseSteps
    {
        [Then(@"The response status is: ([a-zA-Z]*)")]
        public void ThenTheResponseStatusIs(string statusCodeString)
        {
            var statusCode = ScenarioContext.Current.Get<HttpStatusCode>(ScenarioContextKeys.HttpResponseStatusCode);
            HttpStatusCode expectedStatusCode;
            Enum.TryParse(statusCodeString, out expectedStatusCode).Should().BeTrue();
            statusCode.Should().Be(expectedStatusCode);
        }

        [Then(@"The response status is: ([a-zA-Z]*) with response message: (.*)")]
        public void ThenTheResponseStatusIs(string statusCodeString, string responseMessage)
        {
            ThenTheResponseStatusIs(statusCodeString);
            var message = ScenarioContext.Current.Get<ResponseMessage>(ScenarioContextKeys.HttpResponseMessage);
            message.Message.Should().Be(responseMessage);
        }
    }
}