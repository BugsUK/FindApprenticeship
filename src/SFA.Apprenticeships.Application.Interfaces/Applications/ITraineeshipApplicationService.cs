namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;

    public interface ITraineeshipApplicationService : ICommonApplicationService
    {
        IEnumerable<TraineeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId);

        TraineeshipApplicationDetail GetApplication(Guid applicationId);

        TraineeshipApplicationDetail GetApplicationForReview(Guid applicationId);
    }
}
