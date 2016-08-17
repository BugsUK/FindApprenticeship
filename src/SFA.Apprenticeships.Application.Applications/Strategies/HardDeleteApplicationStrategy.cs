namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using System;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;

    public class HardDeleteApplicationStrategy : IHardDeleteApplicationStrategy
    {
        private readonly ILogService _logService;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;

        public HardDeleteApplicationStrategy(
            ILogService logService,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository)
        {
            _logService = logService;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
        }

        public void Delete(VacancyType vacancyType, Guid applicationId)
        {
            _logService.Info("Deleting application: type={0}, id={1}", vacancyType, applicationId);

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

            _logService.Info("Deleted application: type={0}, id={1}", vacancyType, applicationId);
        }
    }
}
