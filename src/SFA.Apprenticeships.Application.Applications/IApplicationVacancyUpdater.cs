namespace SFA.Apprenticeships.Application.Applications
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IApplicationVacancyUpdater
    {
        void Update(Guid candidateId, int vacancyId, VacancyDetail vacancyDetail);
    }
}
