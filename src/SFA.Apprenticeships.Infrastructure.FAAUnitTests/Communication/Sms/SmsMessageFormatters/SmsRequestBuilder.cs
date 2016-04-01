namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;

    public class SmsRequestBuilder
    {
        public const string ToNumber = "0123456789";

        private string _toNumber = ToNumber;
        private MessageTypes _messageType;
        private IEnumerable<CommunicationToken> _tokens = new List<CommunicationToken>(0);

        public SmsRequestBuilder WithToNumber(string toNumber)
        {
            _toNumber = toNumber;
            return this;
        }

        public SmsRequestBuilder WithMessageType(MessageTypes messageType)
        {
            _messageType = messageType;
            return this;
        }

        public SmsRequestBuilder WithTokens(IEnumerable<CommunicationToken> tokens)
        {
            _tokens = tokens;
            return this;
        }

        public SmsRequest Build()
        {
            var smsRequest = new SmsRequest
            {
                ToNumber = _toNumber,
                MessageType = _messageType,
                Tokens = _tokens,
            };
            return smsRequest;
        } 
    }
}