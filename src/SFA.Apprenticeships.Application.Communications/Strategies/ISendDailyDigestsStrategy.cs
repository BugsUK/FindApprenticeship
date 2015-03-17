namespace SFA.Apprenticeships.Application.Communications.Strategies
{
    using System;

    public interface ISendDailyDigestsStrategy
    {
        void SendDailyDigests(Guid batchId);
    }
}
