namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System.Collections.Generic;
    using Messaging;

    /// <summary>
    /// DTO to represent an SMS that should be sent
    /// </summary>
    public class SmsRequest : BaseRequest
    {
        public string ToNumber { get; set; }

        public MessageTypes MessageType { get; set; }

        public IEnumerable<CommunicationToken> Tokens { get; set; }
    }
}
