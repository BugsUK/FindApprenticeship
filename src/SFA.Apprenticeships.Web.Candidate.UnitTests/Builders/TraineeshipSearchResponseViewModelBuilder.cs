namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Linq;
    using Candidate.ViewModels.VacancySearch;
    using Ploeh.AutoFixture;

    public class TraineeshipSearchResponseViewModelBuilder
    {
        private TraineeshipSearchViewModel _vacancySearchViewModel;
        private int _totalHits;
        private int _numberOfPositions;

        public TraineeshipSearchResponseViewModelBuilder()
        {
            _vacancySearchViewModel = new TraineeshipSearchViewModelBuilder().Build();
        }

        public TraineeshipSearchResponseViewModelBuilder WithVacancySearch(TraineeshipSearchViewModel vacancySearchViewModel)
        {
            _vacancySearchViewModel = vacancySearchViewModel;
            return this;
        }

        public TraineeshipSearchResponseViewModelBuilder WithTotalHits(int totalHits)
        {
            _totalHits = totalHits;
            return this;
        }

        public TraineeshipSearchResponseViewModelBuilder WithNumberOfPositions(int numberOfPositions)
        {
            _numberOfPositions = numberOfPositions;
            return this;
        }

        public TraineeshipSearchResponseViewModel Build()
        {
            var vacancies = new Fixture()
                .Build<TraineeshipVacancySummaryViewModel>()
                .With(fixture => fixture.NumberOfPositions, _numberOfPositions)
                .CreateMany(_totalHits)
                .ToList();

            var viewModel = new TraineeshipSearchResponseViewModel
            {
                VacancySearch = _vacancySearchViewModel,
                Vacancies = vacancies,
                TotalHits = _totalHits,
            };

            return viewModel;
        }
    }
}