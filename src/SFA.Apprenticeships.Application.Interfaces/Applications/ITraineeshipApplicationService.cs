namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    using Domain.Entities.Applications;
    using System;
    using System.Collections.Generic;

    public interface ITraineeshipApplicationService : ICommonApplicationService
    {
        IEnumerable<TraineeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId);

        TraineeshipApplicationDetail GetApplication(Guid applicationId);

        TraineeshipApplicationDetail GetApplicationForReview(Guid applicationId);
        void SetStateInProgress(Guid applicationId);
        void SetStateSubmitted(Guid applicationId);
    }
}