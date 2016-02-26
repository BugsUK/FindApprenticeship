namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Common.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies;

    public class ApprenticeshipVacancyDetailViewModelBuilder
    {
        private VacancyStatuses _vacancyStatus = VacancyStatuses.Live;

        public ApprenticeshipVacancyDetailViewModelBuilder WithVacancyStatus(VacancyStatuses vacancyStatus)
        {
            _vacancyStatus = vacancyStatus;
            return this;
        }

        public ApprenticeshipVacancyDetailViewModel Build()
        {
            var model = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyStatus = _vacancyStatus
            };

            return model;
        }
    }
}