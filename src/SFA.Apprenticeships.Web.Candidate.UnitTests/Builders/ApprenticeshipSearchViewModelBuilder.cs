namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Collections.Generic;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators.Search;
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;

    public class ApprenticeshipSearchViewModelBuilder
    {
        private ApprenticeshipSearchMode _searchMode;
        private string _keywords;
        private string _location;
        private int _withinDistance;
        private string _apprenticeshipLevel = "All";
        private string _category;
        private string[] _subCategories;
        private string _viewModelMessage;
        private string _searchField = "All";
        private List<Category> _categories;
        private double? _latitude;
        private double? _longitude;
        private SavedSearchViewModel[] _savedSearches;
        private VacancySearchSortType _sortType;
        private bool _displaySubCategory = true;
        private bool _displayDescription = true;
        private bool _displayDistance = true;
        private bool _displayClosingDate = true;
        private bool _displayStartDate = true;
        private bool _displayApprenticeshipLevel;
        private bool _displayWage;

        public ApprenticeshipSearchViewModel Build()
        {
            var viewModel = new ApprenticeshipSearchViewModel
            {
                SearchMode = _searchMode,
                Keywords = _keywords,
                Location = _location,
                Latitude = _latitude,
                Longitude = _longitude,
                WithinDistance = _withinDistance,
                ApprenticeshipLevel = _apprenticeshipLevel,
                Category = _category,
                SubCategories = _subCategories,
                SearchField = _searchField,

                SortTypes = SearchMediatorBase.GetSortTypes(),
                SortType = _sortType,

                ResultsPerPageSelectList = SearchMediatorBase.GetResultsPerPageSelectList(5),
                Categories = _categories,
                ViewModelMessage = _viewModelMessage,

                SavedSearches = _savedSearches,

                DisplaySubCategory = _displaySubCategory,
                DisplayDescription = _displayDescription,
                DisplayDistance = _displayDistance,
                DisplayClosingDate = _displayClosingDate,
                DisplayStartDate = _displayStartDate,
                DisplayApprenticeshipLevel = _displayApprenticeshipLevel,
                DisplayWage = _displayWage
            };

            if (viewModel.Latitude.HasValue && viewModel.Longitude.HasValue)
            {
                viewModel.Hash = viewModel.LatLonLocHash();
            }

            return viewModel;
        }

        public ApprenticeshipSearchViewModelBuilder WithSearchMode(ApprenticeshipSearchMode searchMode)
        {
            _searchMode = searchMode;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithKeywords(string keywords)
        {
            _keywords = keywords;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithLocation(string location)
        {
            _location = location;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithinDistance(int withinDistance)
        {
            _withinDistance = withinDistance;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithApprenticeshipLevel(string apprenticeshipLevel)
        {
            _apprenticeshipLevel = apprenticeshipLevel;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithCategory(string category)
        {
            _category = category;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithSubCategories(string[] subCategories)
        {
            _subCategories = subCategories;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithSearchField(string searchField)
        {
            _searchField = searchField;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithMessage(string viewModelMessage)
        {
            _viewModelMessage = viewModelMessage;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithCategories(List<Category> categories)
        {
            _categories = categories;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithLatLong(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithSavedSearches(SavedSearchViewModel[] savedSearches)
        {
            _savedSearches = savedSearches;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithSortType(VacancySearchSortType sortType)
        {
            _sortType = sortType;
            return this;
        }

        public ApprenticeshipSearchViewModelBuilder WithDisplay(bool displaySubCategory, bool displayDescription, bool displayDistance, bool displayClosingDate, bool displayStartDate, bool displayApprenticeshipLevel, bool displayWage)
        {
            _displaySubCategory = displaySubCategory;
            _displayDescription = displayDescription;
            _displayDistance = displayDistance;
            _displayClosingDate = displayClosingDate;
            _displayStartDate = displayStartDate;
            _displayApprenticeshipLevel = displayApprenticeshipLevel;
            _displayWage = displayWage;
            return this;
        }
    }
}