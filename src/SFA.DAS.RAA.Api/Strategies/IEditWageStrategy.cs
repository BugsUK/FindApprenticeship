namespace SFA.DAS.RAA.Api.Strategies
{
    using System;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;

    public interface IEditWageStrategy
    {
        Vacancy EditWage(WageUpdate wageUpdate, int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null);
    }
}