namespace SFA.Apprenticeships.Application.Services.Communication.Strategies.Interfaces
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;

    public interface ISendGlaEmployerEnquiryStrategy
    {
        void Send(MessageTypes messageType, IEnumerable<CommunicationToken> tokens); 
    }
}