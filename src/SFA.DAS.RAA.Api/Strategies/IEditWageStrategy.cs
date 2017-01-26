namespace SFA.DAS.RAA.Api.Strategies
{
    using System;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Models;

    public interface IEditWageStrategy
    {
        Vacancy EditWage(WageUpdate wageUpdate, VacancyIdentifier vacancyIdentifier);
    }
}