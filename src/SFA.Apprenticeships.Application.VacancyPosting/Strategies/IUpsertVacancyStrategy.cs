namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities.Raa.Vacancies;
    using System;

    public interface IUpsertVacancyStrategy
    {
        Vacancy UpsertVacancy(Vacancy vacancy, Func<Vacancy, Vacancy> operation);
        Vacancy UpsertVacancyForAdmin(Vacancy vacancy, Func<Vacancy, Vacancy> operation);
    }
}