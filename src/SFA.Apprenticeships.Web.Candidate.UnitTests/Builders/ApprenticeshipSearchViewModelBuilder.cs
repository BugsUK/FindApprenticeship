namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.Mediators.Search;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class ApprenticeshipSearchViewModelBuilder
    {
        private ApprenticeshipSearchMode _searchMode;
        private string _keywords;
        private string _location;
        private int _withinDistance;
        private string _apprenticeshipLevel;
        private string _category;
        private string[] _subCategories;
        private string _viewModelMessage;

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

                SortTypes = SearchMediatorBase.GetSortTypes(),
                ResultsPerPageSelectList = SearchMediatorBase.GetResultsPerPageSelectList(5),
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

        public ApprenticeshipSearchViewModelBuilder WithMessage(string viewModelMessage)
        {
            _viewModelMessage = viewModelMessage;
            return this;
        }
    }
}