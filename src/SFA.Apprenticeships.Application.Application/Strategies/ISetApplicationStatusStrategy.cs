namespace SFA.Apprenticeships.Application.Application.Strategies
{
    using System;

    /// <summary>
    /// TODO: consider merging this with IApplicationStatusUpdateStrategy
    /// </summary>
    public interface ISetApplicationStatusStrategy
    {
        void SetSuccessfulDecision(Guid applicationId);

        void SetUnsuccessfulDecision(Guid applicationId);

        void RevertToViewed(Guid applicationId);
    }
}