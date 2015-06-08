namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using System;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;

    public class AuditApplicationDetailStrategy : IAuditApplicationDetailStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IAuditRepository _auditRepository;

        public AuditApplicationDetailStrategy(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IAuditRepository auditRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _auditRepository = auditRepository;
        }

        public void Audit(VacancyType vacancyType, Guid applicationId, string auditEventTypeCode)
        {
            object application;

            switch (vacancyType)
            {
                case VacancyType.Apprenticeship:
                    application = _apprenticeshipApplicationReadRepository.Get(applicationId);
                    break;

                case VacancyType.Traineeship:
                    application = _traineeshipApplicationReadRepository.Get(applicationId);
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Unknown vacancy type: {0}.", vacancyType));
            }

            _auditRepository.Audit(application, auditEventTypeCode, applicationId);
        }
    }
}
