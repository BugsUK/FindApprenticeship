namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using Candidate.ViewModels.VacancySearch;
    using Ploeh.AutoFixture;

    public class ApprenticeshipSearchResponseViewModelBuilder
    {
        private int _totalLocalHits;
        private int _totalNationalHits;

        private ApprenticeshipSearchViewModel _vacancySearchViewModel;

        public ApprenticeshipSearchResponseViewModelBuilder()
        {
            _vacancySearchViewModel = new ApprenticeshipSearchViewModelBuilder().Build();
        }

        public ApprenticeshipSearchResponseViewModelBuilder WithTotalLocalHits(int totalLocalHits)
        {
            _totalLocalHits = totalLocalHits;
            return this;
        }

        public ApprenticeshipSearchResponseViewModelBuilder WithTotalNationalHits(int totalNationalHits)
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
            var vacancies = new List<ApprenticeshipVacancySummaryViewModel>();
            var hits = _totalLocalHits > 0 ? _totalLocalHits : _totalNationalHits;

            if (hits > 0)
            {
                vacancies = new Fixture()
                    .Build<ApprenticeshipVacancySummaryViewModel>()
                    .CreateMany(hits)
                    .ToList();                
            }

            var viewModel = new ApprenticeshipSearchResponseViewModel
            {
                TotalLocalHits = _totalLocalHits,
                TotalNationalHits = _totalNationalHits,
                VacancySearch = _vacancySearchViewModel,
                Vacancies = vacancies
            };

            return viewModel;
        }
    }
}