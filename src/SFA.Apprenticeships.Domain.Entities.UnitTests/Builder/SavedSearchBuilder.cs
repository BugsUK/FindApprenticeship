namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Builder
{
    using Entities.Candidates;
    using Entities.Vacancies.Apprenticeships;

    public class SavedSearchBuilder
    {
        private ApprenticeshipSearchMode _searchMode;
        private string _keywords;
        private string _location;
        private int _withinDistance;
        private string _apprenticeshipLevel;
        private string _category;
        private string[] _subCategories;

        public SavedSearch Build()
        {
            var savedSearch = new SavedSearch
            {
                SearchMode = _searchMode,
                Keywords = _keywords,
                Location = _location,
                WithinDistance = _withinDistance,
                ApprenticeshipLevel = _apprenticeshipLevel,
                Category = _category,
                SubCategories = _subCategories
            };

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
    }
}