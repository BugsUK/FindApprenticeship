namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies;
    using Ploeh.AutoFixture;

    public class ApprenticeshipSearchResponseViewModelBuilder
    {
        private int _totalLocalHits;
        private int _totalNationalHits;
        private bool _isPositiveAboutDisability;

        private ApprenticeshipSearchViewModel _vacancySearchViewModel;
        private int _numberOfPositions;

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

        public ApprenticeshipSearchResponseViewModelBuilder WithIsPositiveAboutDisability(bool isPositiveAboutDisability)
        {
            _isPositiveAboutDisability = isPositiveAboutDisability;
            return this;
        }

        public ApprenticeshipSearchResponseViewModelBuilder WithNumberOfPositions(int numberOfPositions)
        {
            _numberOfPositions = numberOfPositions;
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
                    .With(x => x.IsPositiveAboutDisability, _isPositiveAboutDisability)
                    .With(x => x.NumberOfPositions, _numberOfPositions)
                    .With(x => x.VacancyLocationType, VacancyLocationType.NonNational)
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