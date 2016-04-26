namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;

    public interface ITraineeshipApplicationService
    {
        IEnumerable<TraineeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId);

        int GetApplicationCount(int vacancyId);

        int GetNewApplicationCount(int vacancyId);

        TraineeshipApplicationDetail GetApplication(Guid applicationId);

        TraineeshipApplicationDetail GetApplicationForReview(Guid applicationId);

        void UpdateApplicationNotes(Guid applicationId, string notes);
        
        int GetNewApplicationsCount(List<int> liveVacancyIds);
    }
}
