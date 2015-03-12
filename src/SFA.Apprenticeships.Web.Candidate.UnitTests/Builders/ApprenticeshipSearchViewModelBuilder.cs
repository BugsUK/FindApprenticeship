namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Collections.Generic;
    using Candidate.Mediators.Search;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;

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

        public ApprenticeshipSearchViewModel Build()
        {
            var viewModel = new ApprenticeshipSearchViewModel
            {
                SearchMode = _searchMode,
                Keywords = _keywords,
                Location = _location,
                WithinDistance = _withinDistance,
                ApprenticeshipLevel = _apprenticeshipLevel,
                Category = _category,
                SubCategories = _subCategories,
                SearchField = _searchField,

                SortTypes = SearchMediatorBase.GetSortTypes(),
                ResultsPerPageSelectList = SearchMediatorBase.GetResultsPerPageSelectList(5),
                Categories = _categories,
                ViewModelMessage = _viewModelMessage
            };
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
    }
}