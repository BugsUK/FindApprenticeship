namespace SFA.DAS.RAA.Api.Providers
{
    using System;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;

    public interface IVacancyProvider
    {
        Vacancy Get(int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null);
    }
}