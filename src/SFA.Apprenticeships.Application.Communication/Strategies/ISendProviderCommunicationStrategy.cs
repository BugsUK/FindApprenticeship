namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Interfaces.Communications;

    public interface ISendProviderCommunicationStrategy
    {
        void Send(string username, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}
