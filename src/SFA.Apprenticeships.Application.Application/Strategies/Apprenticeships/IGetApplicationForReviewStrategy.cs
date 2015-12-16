namespace SFA.Apprenticeships.Application.Application.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;

    public interface IGetApplicationForReviewStrategy
    {
        ApprenticeshipApplicationDetail GetApplicationForReview(Guid applicationId);
    }
}