namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Interfaces.Communications;

    public interface ISendEmployerCommunicationStrategy
    {
        void Send(MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}