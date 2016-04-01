namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Email.EmailMessageFormatters.Builders
{
    using System;
    using System.Collections.Generic;
    using Application.Communications;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Domain.Entities.UnitTests.Builder;

    public class SavedSearchAlertEmailRequestBuilder
    {
        private List<SavedSearchAlert> _savedSearchAlerts;

        public SavedSearchAlertEmailRequestBuilder()
        {
            _savedSearchAlerts = new List<SavedSearchAlert>();
        }

        public SavedSearchAlertEmailRequestBuilder WithSavedSearchAlerts(List<SavedSearchAlert> savedSearchAlerts)
        {
            _savedSearchAlerts = savedSearchAlerts;
            return this;
        }

        public EmailRequest Build()
        {
            var candidateId = Guid.NewGuid();
 
            var candidate = new CandidateBuilder(candidateId)
                .FirstName("Jane Doe")
                .Build();
 
            var communicationRequest = CommunicationRequestFactory.GetSavedSearchAlertCommunicationRequest(candidate, _savedSearchAlerts);
            var emailRequest = new EmailRequestBuilder().WithMessageType(MessageTypes.SavedSearchAlert).WithTokens(communicationRequest.Tokens).Build();

            return emailRequest;
        }
    }
}