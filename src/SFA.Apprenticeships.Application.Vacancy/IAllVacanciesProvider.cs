namespace SFA.Apprenticeships.Application.Vacancy
{
    using System.Collections.Generic;

    public interface IAllVacanciesProvider<TVacancySearchResponse>
        where TVacancySearchResponse : class
    {
        IEnumerable<TVacancySearchResponse> GetAllVacancies();
    }
}
