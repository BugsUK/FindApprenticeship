namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.VacancySearch;

    public class ApprenticeshipSearchResponseViewModelBuilder
    {
        private long _totalLocalHits;
        private long _totalNationalHits;
        private ApprenticeshipSearchViewModel _vacancySearchViewModel;

        public ApprenticeshipSearchResponseViewModelBuilder()
        {
            _vacancySearchViewModel = new ApprenticeshipSearchViewModelBuilder().Build();
        }

        public ApprenticeshipSearchResponseViewModelBuilder WithTotalLocalHits(long totalLocalHits)
        {
            _totalLocalHits = totalLocalHits;
            return this;
        }

        public ApprenticeshipSearchResponseViewModelBuilder WithTotalNationalHits(long totalNationalHits)
        {
            _totalNationalHits = totalNationalHits;
            return this;
        }

        public ApprenticeshipSearchResponseViewModelBuilder WithVacancySearch(ApprenticeshipSearchViewModel vacancySearchViewModel)
        {
            _vacancySearchViewModel = vacancySearchViewModel;
            return this;
        }

        public ApprenticeshipSearchResponseViewModel Build()
        {
            var viewModel = new ApprenticeshipSearchResponseViewModel
            {
                TotalLocalHits = _totalLocalHits,
                TotalNationalHits = _totalNationalHits,
                VacancySearch = _vacancySearchViewModel
            };
            return viewModel;
        }
    }
}