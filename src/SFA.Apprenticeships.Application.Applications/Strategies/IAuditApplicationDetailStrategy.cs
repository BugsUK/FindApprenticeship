namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IAuditApplicationDetailStrategy
    {
        void Audit(VacancyType vacancyType, Guid applicationId, string auditEventTypeCode);
    }
}
