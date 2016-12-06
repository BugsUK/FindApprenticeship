namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Builder
{
    using System;
    using Entities.Candidates;
    using Entities.Vacancies;

    public class SavedSearchBuilder
    {
        private ApprenticeshipSearchMode _searchMode;
        private string _keywords;
        private string _location;
        private int _withinDistance;
        private string _apprenticeshipLevel = "All";
        private string _category;
        private string[] _subCategories;
        private string _categoryFullName;
        private bool _alertsEnabled = true;
        private string _searchField = "All";
        private double? _latitude;
        private double? _longitude;
        private string _subCategoriesFullNames;

        public SavedSearch Build()
        {
            var savedSearch = new SavedSearch
            {
                EntityId = Guid.NewGuid(),
                SearchMode = _searchMode,
                Keywords = _keywords,
                Location = _location,
                Latitude = _latitude,
                Longitude = _longitude,
                WithinDistance = _withinDistance,
                ApprenticeshipLevel = _apprenticeshipLevel,
                Category = _category,
                CategoryFullName = _categoryFullName,
                SubCategories = _subCategories,
                SubCategoriesFullName = _subCategoriesFullNames,
                SearchField = _searchField,
                AlertsEnabled = _alertsEnabled
            };

            if (savedSearch.HasGeoPoint())
            {
                savedSearch.Hash = savedSearch.GetLatLonLocHash();
            }

            return savedSearch;
        }

        public SavedSearchBuilder WithSearchMode(ApprenticeshipSearchMode searchMode)
        {
            _searchMode = searchMode;
            return this;
        }

        public SavedSearchBuilder WithKeywords(string keywords)
        {
            _keywords = keywords;
            return this;
        }

        public SavedSearchBuilder WithLocation(string location)
        {
            _location = location;
            return this;
        }

        public SavedSearchBuilder WithinDistance(int withinDistance)
        {
            _withinDistance = withinDistance;
            return this;
        }

        public SavedSearchBuilder WithApprenticeshipLevel(string apprenticeshipLevel)
        {
            _apprenticeshipLevel = apprenticeshipLevel;
            return this;
        }

        public SavedSearchBuilder WithCategory(string category)
        {
            _category = category;
            return this;
        }

        public SavedSearchBuilder WithSubCategories(string[] subCategories)
        {
            _subCategories = subCategories;
            return this;
        }

        public SavedSearchBuilder WithCategoryFullName(string categoryFullName)
        {
            _categoryFullName = categoryFullName;
            return this;
        }

        public SavedSearchBuilder WithAlertsEnabled(bool alertsEnabled)
        {
            _alertsEnabled = alertsEnabled;
            return this;
        }

        public SavedSearchBuilder WithSearchField(string searchField)
        {
            _searchField = searchField;
            return this;
        }

        public SavedSearchBuilder WithLatLong(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
            return this;
        }

        public SavedSearchBuilder WithSubCategoriesFullNames(string subCategoriesFullNames)
        {
            _subCategoriesFullNames = subCategoriesFullNames;
            return this;
        }
    }
}