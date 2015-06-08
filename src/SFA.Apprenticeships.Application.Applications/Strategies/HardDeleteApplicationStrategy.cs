namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using System;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;

    public class HardDeleteApplicationStrategy : IHardDeleteApplicationStrategy
    {
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;

        public HardDeleteApplicationStrategy(
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository)
        {
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
        }

        public void Delete(VacancyType vacancyType, Guid applicationId)
        {
            switch (vacancyType)
            {
                case VacancyType.Apprenticeship:
                    _apprenticeshipApplicationWriteRepository.Delete(applicationId);
                    break;
                case VacancyType.Traineeship:
                    _traineeshipApplicationWriteRepository.Delete(applicationId);
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Unknown vacancy type: {0}.", vacancyType));
            }
        }
    }
}
