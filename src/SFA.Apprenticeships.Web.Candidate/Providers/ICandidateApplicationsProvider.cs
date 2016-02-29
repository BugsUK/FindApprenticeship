namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using ViewModels.Applications;
    using ViewModels.MyApplications;

    public interface ICandidateApplicationsProvider
    {
        MyApplicationsViewModel GetCandidateApplications(Guid candidateId);

        TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId);

        void RecalculateSavedAndDraftCount(Guid candidateId, IList<ApprenticeshipApplicationSummary> summaries);
    }
}