namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Common.ViewModels.Applications;
    using ViewModels.Applications;

    public interface ITraineeshipApplicationProvider
    {
        TraineeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId);

        TraineeshipApplicationViewModel GetApplicationViewModelEx(Guid candidateId, int vacancyId);

        TraineeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId, TraineeshipApplicationViewModel traineeshipApplicationViewModel);

        WhatHappensNextTraineeshipViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId);

        TraineeshipApplicationViewModel PatchApplicationViewModel(Guid candidateId, TraineeshipApplicationViewModel savedModel, TraineeshipApplicationViewModel submittedModel);
    }
}
