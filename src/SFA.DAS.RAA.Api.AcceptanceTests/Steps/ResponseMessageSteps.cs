
namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using Constants;
    using FluentAssertions;
    using Models;
    using TechTalk.SpecFlow;

    [Binding]
    public class ResponseMessageSteps
    {
        [Then(@"The validation errors contain:")]
        public void ThenTheValidationErrorsContain(Table table)
        {
            var message = ScenarioContext.Current.Get<ResponseMessage>(ScenarioContextKeys.HttpResponseMessage);

            foreach (var tableRow in table.Rows)
            {
                var property = tableRow["Property"];
                var error = tableRow["Error"];
                message.ModelState[property][0].Should().Be(error);
            }
        }
    }
}
