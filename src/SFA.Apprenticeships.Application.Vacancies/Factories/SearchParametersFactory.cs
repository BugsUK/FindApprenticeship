namespace SFA.Apprenticeships.Application.Vacancies.Factories
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Interfaces.Vacancies;

    public class SearchParametersFactory
    {
        public static ApprenticeshipSearchParameters Create(SavedSearch savedSearch)
        {
            ApprenticeshipSearchField searchField;
            if (!Enum.TryParse(savedSearch.SearchField, out searchField))
            {
                searchField = ApprenticeshipSearchField.All;
            }

            var location = new Location { Name = savedSearch.Location };

            if (savedSearch.HasGeoPoint())
            {
                location.GeoPoint = new GeoPoint
                {
                    // ReSharper disable PossibleInvalidOperationException HasGeoPoint() checks for this
                    Latitude = savedSearch.Latitude.Value,
                    Longitude = savedSearch.Longitude.Value
                    // ReSharper restore PossibleInvalidOperationException
                };
            }

            var parameters = new ApprenticeshipSearchParameters
            {
                PageNumber = 1,
                PageSize = 5,

                Location = location,
                SearchRadius = savedSearch.WithinDistance,
                SortType = VacancySearchSortType.RecentlyAdded,
                ApprenticeshipLevel = savedSearch.ApprenticeshipLevel,

                VacancyLocationType = VacancyLocationType.NonNational,
                SearchField = searchField
            };

            switch (savedSearch.SearchMode)
            {
                case ApprenticeshipSearchMode.Keyword:
                    parameters.Keywords = savedSearch.Keywords;
                    break;
                case ApprenticeshipSearchMode.Category:
                    parameters.CategoryCode = savedSearch.Category;
                    parameters.SubCategoryCodes = savedSearch.SubCategories;
                    parameters.SearchField = ApprenticeshipSearchField.All;
                    break;
            }

            return parameters;
        }
    }
}