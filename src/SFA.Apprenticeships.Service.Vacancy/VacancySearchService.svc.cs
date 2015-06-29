namespace SFA.Apprenticeships.Service.Vacancy
{
    using System.Linq;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Types;

    public class VacancySearchService : IVacancySearchService
    {
        public SearchResponse Search(SearchRequest request)
        {
            var results = new SearchProvider().Search(request);

            return new SearchResponse
            {
                Request = request,
                SearchResults = results.Results.Select(each => new ApprenticeshipSearchResponse
                {
                    Title = each.Title,
                    Description = each.Description,
                    EmployerName = each.EmployerName,
                    ClosingDate = each.ClosingDate,
                    Location = each.Location
                })
            };
        }
    }
}
