namespace SFA.Apprenticeships.Application.Application.Strategies.Traineeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class GetApplicationForReviewStrategy : IGetApplicationForReviewStrategy
    {
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;

        public GetApplicationForReviewStrategy(ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository, ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
        }

        public TraineeshipApplicationDetail GetApplicationForReview(Guid applicationId)
        {
            var application = _traineeshipApplicationReadRepository.Get(applicationId);
            
            return application;
        }
    }
}
