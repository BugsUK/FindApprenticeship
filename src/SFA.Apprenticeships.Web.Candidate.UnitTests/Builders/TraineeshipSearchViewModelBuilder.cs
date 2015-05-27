namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Web.Mvc;
    using Application.Interfaces.Vacancies;
    using Candidate.ViewModels.VacancySearch;

    public class TraineeshipSearchViewModelBuilder
    {
        private VacancySearchSortType _sortType;

        public TraineeshipSearchViewModel Build()
        {
            var viewModel = new TraineeshipSearchViewModel
            {
                SortType = _sortType,
                SortTypes = new SelectList(new string[0])
            };

            return viewModel;
        }

        public TraineeshipSearchViewModelBuilder WithSortType(VacancySearchSortType sortType)
        {
            _sortType = sortType;
            return this;
        }
    }
}