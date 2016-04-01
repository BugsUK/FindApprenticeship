namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Builders
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Communications;

    public class CommunicationRequestBuilder
    {
        public const string DefaultTestEmailAddress = "jane.doe@example.com";
        public const string DefaultTestMobileNumber = "07999999999";

        private readonly MessageTypes _messageType;
        private readonly Guid _candidateId;

        private string _mobileNumber = DefaultTestMobileNumber;
        private string _emailAddress = DefaultTestEmailAddress;

        private readonly List<CommunicationToken> _tokens = new List<CommunicationToken>();

        public CommunicationRequestBuilder(MessageTypes messageType, Guid candidateId)
        {
            _messageType = messageType;
            _candidateId = candidateId;
        }

        public CommunicationRequestBuilder WithMobileNumber(string mobileNumber)
        {
            _mobileNumber = mobileNumber;
            return this;
        }

        public CommunicationRequestBuilder WithEmailAddress(string emailAddress)
        {
            _emailAddress = emailAddress;
            return this;
        }

        public CommunicationRequestBuilder WithToken(CommunicationTokens token, string value)
        {
            _tokens.Add(new CommunicationToken(token, value));
            return this;
        }

        public CommunicationRequest Build()
        {
            _tokens.Add(new CommunicationToken(CommunicationTokens.RecipientEmailAddress, _emailAddress));
            _tokens.Add(new CommunicationToken(CommunicationTokens.CandidateMobileNumber, _mobileNumber));

            return new CommunicationRequest
            {
                MessageType = _messageType,
                Tokens = _tokens,
                EntityId = _candidateId
            };
        }
    }
}
