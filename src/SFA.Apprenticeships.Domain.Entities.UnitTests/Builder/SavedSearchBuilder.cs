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
        private string _apprenticeshipLevel = "All";
        private string _category;
        private string[] _subCategories;
        private string _categoryFullName;

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
                CategoryFullName = _categoryFullName,
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

        public SavedSearchBuilder WithCategoryFullName(string categoryFullName)
        {
            _categoryFullName = categoryFullName;
            return this;
        }
    }
}