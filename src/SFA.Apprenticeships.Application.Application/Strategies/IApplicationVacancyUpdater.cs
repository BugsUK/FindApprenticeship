namespace SFA.Apprenticeships.Application.Application.Strategies
{
    using Domain.Entities.Vacancies;
    using System;

    public interface IApplicationVacancyUpdater
    {
        void Update(Guid candidateId, int vacancyId, VacancyDetail vacancyDetail);
    }
}
