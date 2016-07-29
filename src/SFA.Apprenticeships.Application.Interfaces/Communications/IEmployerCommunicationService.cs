namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System.Collections.Generic;

    public interface IEmployerCommunicationService
    {
        void SendMessageToEmployer(string recipientEmailAddress, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}