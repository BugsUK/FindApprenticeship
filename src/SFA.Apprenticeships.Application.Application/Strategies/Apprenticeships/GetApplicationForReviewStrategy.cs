namespace SFA.Apprenticeships.Application.Application.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class GetApplicationForReviewStrategy : IGetApplicationForReviewStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;

        public GetApplicationForReviewStrategy(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
        }

        public ApprenticeshipApplicationDetail GetApplicationForReview(Guid applicationId)
        {
            var application = _apprenticeshipApplicationReadRepository.Get(applicationId);

            return application;
        }
    }
}
