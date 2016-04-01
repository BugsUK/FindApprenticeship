namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Email.EmailMessageFormatters.Builders
{
    using System;
    using System.Collections.Generic;
    using Application.Communications;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Domain.Entities.UnitTests.Builder;

    public class DailyDigestEmailRequestBuilder
    {
        private List<ExpiringApprenticeshipApplicationDraft> _expiringDrafts;
        private List<ApplicationStatusAlert> _applicationStatusAlerts;

        public DailyDigestEmailRequestBuilder()
        {
            _expiringDrafts = new List<ExpiringApprenticeshipApplicationDraft>();
        }

        public DailyDigestEmailRequestBuilder WithExpiringDrafts(List<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            _expiringDrafts = expiringDrafts;
            return this;
        }

        public DailyDigestEmailRequestBuilder WithApplicationStatusAlerts(List<ApplicationStatusAlert> applicationStatusAlerts)
        {
            _applicationStatusAlerts = applicationStatusAlerts;
            return this;
        }

        public EmailRequest Build()
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId).Build();
            var communicationRequest = CommunicationRequestFactory.GetDailyDigestCommunicationRequest(candidate, _expiringDrafts, _applicationStatusAlerts);
            var emailRequest = new EmailRequestBuilder().WithMessageType(MessageTypes.DailyDigest).WithTokens(communicationRequest.Tokens).Build();
            return emailRequest;
        }
    }
}