namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System.Collections.Generic;
    using Domain.Entities;

    public interface ICommunciationService
    {
        void SendMessageToHelpdesk(MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }

}