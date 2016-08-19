namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System;
    using Domain.Entities.Raa.Vacancies;

    public interface IGetVacancyStrategies
    {
        Vacancy GetVacancyByReferenceNumber(int vacancyReferenceNumber);

        Vacancy GetVacancyByGuid(Guid vacancyGuid);

        Vacancy GetVacancyById(int vacancyId);
    }
}