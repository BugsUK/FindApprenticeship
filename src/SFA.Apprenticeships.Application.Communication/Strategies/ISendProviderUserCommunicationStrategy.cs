namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Interfaces.Communications;

    public interface ISendProviderUserCommunicationStrategy
    {
        void Send(string username, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}
