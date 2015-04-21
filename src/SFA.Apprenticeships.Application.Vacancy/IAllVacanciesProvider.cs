namespace SFA.Apprenticeships.Application.Vacancy
{
    using System.Collections.Generic;

    public interface IAllVacanciesProvider
    {
        IEnumerable<int> GetAllVacancyIds(string indexName);
    }
}
