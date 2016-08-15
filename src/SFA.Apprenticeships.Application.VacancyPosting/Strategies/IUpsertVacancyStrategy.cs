namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System;
    using Domain.Entities.Raa.Vacancies;

    public interface IUpsertVacancyStrategy
    {
        Vacancy UpsertVacancy(Vacancy vacancy, Func<Vacancy, Vacancy> operation);
    }
}