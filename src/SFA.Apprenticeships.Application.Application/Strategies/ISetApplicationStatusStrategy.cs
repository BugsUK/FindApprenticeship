namespace SFA.Apprenticeships.Application.Application.Strategies
{
    using System;

    public interface ISetApplicationStatusStrategy
    {
        void SetSuccessfulDecision(Guid applicationId);

        void SetUnsuccessfulDecision(Guid applicationId);

        void RevertToViewed(Guid applicationId);
    }
}