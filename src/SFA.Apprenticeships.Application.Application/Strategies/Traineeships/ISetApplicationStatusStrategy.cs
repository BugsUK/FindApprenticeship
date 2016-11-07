namespace SFA.Apprenticeships.Application.Application.Strategies.Traineeships
{
    using System;

    /// <summary>
    /// TODO: consider merging this with IApplicationStatusUpdateStrategy
    /// </summary>
    public interface ISetApplicationStatusStrategy
    {
        void SetStateInProgress(Guid applicationId);

        void SetStateSubmitted(Guid applicationId);
    }
}