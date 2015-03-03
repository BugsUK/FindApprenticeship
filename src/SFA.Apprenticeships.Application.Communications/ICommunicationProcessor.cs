namespace SFA.Apprenticeships.Application.Communications
{
    using System;

    public interface ICommunicationProcessor
    {
        void SendDailyCommunications(Guid batchId);
    }
}
