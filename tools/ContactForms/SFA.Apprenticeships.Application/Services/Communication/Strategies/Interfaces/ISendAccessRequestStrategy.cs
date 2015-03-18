namespace SFA.Apprenticeships.Application.Services.Communication.Strategies.Interfaces
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;

    public interface ISendAccessRequestStrategy
    {
        void Send(MessageTypes messageType, IEnumerable<CommunicationToken> tokens);  
    }
}