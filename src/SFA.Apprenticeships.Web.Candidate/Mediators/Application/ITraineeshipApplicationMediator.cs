using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Application
{
    using System;
    using ViewModels.Applications;

    public interface ITraineeshipApplicationMediator
    {
        MediatorResponse<TraineeshipApplicationViewModel> Apply(Guid candidateId, string vacancyIdString);

        MediatorResponse<TraineeshipApplicationViewModel> Submit(Guid candidateId, int vacancyId, TraineeshipApplicationViewModel viewModel);

        MediatorResponse<TraineeshipApplicationViewModel> AddEmptyQualificationRows(TraineeshipApplicationViewModel viewModel);

        MediatorResponse<TraineeshipApplicationViewModel> AddEmptyWorkExperienceRows(TraineeshipApplicationViewModel viewModel);

        MediatorResponse<TraineeshipApplicationViewModel> AddEmptyTrainingCourseRows(TraineeshipApplicationViewModel viewModel);

        MediatorResponse<WhatHappensNextTraineeshipViewModel> WhatHappensNext(Guid candidateId, string vacancyIdString, string vacancyReference, string vacancyTitle);

        MediatorResponse<TraineeshipApplicationViewModel> View(Guid candidateId, int vacancyId);
    }
}