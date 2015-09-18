namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System.Collections.Generic;

    public interface IProviderCommunicationService
    {
        void SendMessageToProviderUser(string username, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}
