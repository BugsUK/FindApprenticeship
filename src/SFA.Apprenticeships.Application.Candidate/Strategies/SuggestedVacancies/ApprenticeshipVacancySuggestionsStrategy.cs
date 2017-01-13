namespace SFA.Apprenticeships.Application.Candidate.Strategies.SuggestedVacancies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Interfaces.Locations;
    using Interfaces.ReferenceData;
    using Interfaces.Search;
    using Interfaces.Vacancies;
    using Vacancy;

    public class ApprenticeshipVacancySuggestionsStrategy : IApprenticeshipVacancySuggestionsStrategy
    {
        private readonly IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> _searchService;
        private readonly IVacancyDataProvider<ApprenticeshipVacancyDetail> _vacancyDataProvider;
        private readonly ILocationSearchService _locationSearchService;
        private readonly IReferenceDataService _referenceDataService;

        public ApprenticeshipVacancySuggestionsStrategy(
            IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> searchService,
            IVacancyDataProvider<ApprenticeshipVacancyDetail> vacancyDataProvider,
            ILocationSearchService locationSearchService,
            IReferenceDataService referenceDataService)
        {
            _searchService = searchService;
            _vacancyDataProvider = vacancyDataProvider;
            _locationSearchService = locationSearchService;
            _referenceDataService = referenceDataService;
        }

        public SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> GetSuggestedApprenticeshipVacancies(
            ApprenticeshipSearchParameters searchParameters, 
            IList<ApprenticeshipApplicationSummary> candidateApplications, 
            int vacancyId)
        {
            var vacancy = _vacancyDataProvider.GetVacancyDetails(vacancyId);
            var vacancySubCategory = _referenceDataService.GetSubCategoryByName(vacancy.SubCategory);

            if (vacancySubCategory == null) { return null; }

            searchParameters.CategoryCode = vacancySubCategory.ParentCategoryCodeName;
            searchParameters.SubCategoryCodes = new[] {vacancySubCategory.CodeName};

            var excludeVacancyIds = candidateApplications.Select(x => x.LegacyVacancyId).ToList();
            excludeVacancyIds.Add(vacancyId);
            searchParameters.ExcludeVacancyIds = excludeVacancyIds;

            if (searchParameters.Location != null)
            {
                if (searchParameters.Location.GeoPoint == null || (searchParameters.Location.GeoPoint.Latitude == 0 && searchParameters.Location.GeoPoint.Longitude == 0))
                {
                    var locations = _locationSearchService.FindLocation(searchParameters.Location.Name);
                    searchParameters.Location = locations != null ? locations.FirstOrDefault() : null;
                }
            }

            if (searchParameters.Location == null)
            {
                searchParameters.Location = new Location
                {
                    Name = vacancy.VacancyAddress.Postcode,
                    GeoPoint = new GeoPoint
                    {
                        Latitude = vacancy.VacancyAddress.GeoPoint.Latitude,
                        Longitude = vacancy.VacancyAddress.GeoPoint.Longitude,
                    }
                };
            }

            var searchResults = _searchService.Search(searchParameters);

            if (searchResults.Total == 0)
            {
                //Widen search to category alone
                searchParameters.SubCategoryCodes = null;
                searchResults = _searchService.Search(searchParameters);
            }

            return searchResults;
        }
    }
}
