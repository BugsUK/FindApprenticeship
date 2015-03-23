namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System.Collections.Generic;
    using Domain.Entities;

    public interface ICommunciationService
    {
        void SendMessage(MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }

}