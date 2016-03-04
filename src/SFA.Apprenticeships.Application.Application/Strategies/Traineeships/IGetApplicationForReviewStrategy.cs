namespace SFA.Apprenticeships.Application.Application.Strategies.Traineeships
{
    using System;
    using Domain.Entities.Applications;

    public interface IGetApplicationForReviewStrategy
    {
        TraineeshipApplicationDetail GetApplicationForReview(Guid applicationId);
    }
}