namespace SFA.Apprenticeships.Application.Vacancies.Factories
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.Apprenticeships;
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

            var parameters = new ApprenticeshipSearchParameters
            {
                PageNumber = 1,
                PageSize = 5,

                Location = new Location {Name = savedSearch.Location},
                SearchRadius = savedSearch.WithinDistance,
                SortType = VacancySearchSortType.RecentlyAdded,
                ApprenticeshipLevel = savedSearch.ApprenticeshipLevel,

                VacancyLocationType = ApprenticeshipLocationType.NonNational,
                SearchField = searchField
            };

            switch (savedSearch.SearchMode)
            {
                case ApprenticeshipSearchMode.Keyword:
                    parameters.Keywords = savedSearch.Keywords;
                    break;
                case ApprenticeshipSearchMode.Category:
                    parameters.Sector = savedSearch.Category;
                    parameters.Frameworks = savedSearch.SubCategories;
                    parameters.SearchField = ApprenticeshipSearchField.All;
                    break;
            }

            return parameters;
        }
    }
}