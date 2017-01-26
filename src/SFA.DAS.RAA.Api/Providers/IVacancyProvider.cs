using SFA.DAS.RAA.Api.Models;

namespace SFA.DAS.RAA.Api.Providers
{
    using Apprenticeships.Domain.Entities.Raa.Vacancies;

    public interface IVacancyProvider
    {
        Vacancy Get(VacancyIdentifier vacancyIdentifier);
    }
}