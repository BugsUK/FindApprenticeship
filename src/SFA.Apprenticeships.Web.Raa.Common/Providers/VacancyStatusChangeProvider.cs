namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories;
    using ViewModels.VacancyStatus;

    public class VacancyStatusChangeProvider : IVacancyStatusChangeProvider
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;

        public VacancyStatusChangeProvider(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository, IVacancyReadRepository vacancyReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _vacancyReadRepository = vacancyReadRepository;
        }

        public ArchiveVacancyViewModel GetArchiveVacancyViewModelByVacancyId(int vacancyId)
        {
            var vacancy = _vacancyReadRepository.Get(vacancyId);
            
            //TODO: Put this somewhwere more common
            var statusesRequiringAction = new List<ApplicationStatuses>()
            {
                ApplicationStatuses.Submitting,
                ApplicationStatuses.InProgress,
                ApplicationStatuses.Submitted
            };

            if (vacancy.VacancyType == VacancyType.Traineeship)
            {
                var traineeshipApplicationSummaries = _traineeshipApplicationReadRepository.GetApplicationSummaries(vacancyId);
                var hasOutstandingActions = (!traineeshipApplicationSummaries.Any() ||
                                             traineeshipApplicationSummaries.Any(
                                                 a => statusesRequiringAction.Contains(a.Status)));
                return new ArchiveVacancyViewModel(hasOutstandingActions, vacancyId);

            }
            else
            {
                var apprenticeshipApplicationSummaries = _apprenticeshipApplicationReadRepository.GetApplicationSummaries(vacancyId);
                var hasOutstandingActions = (!apprenticeshipApplicationSummaries.Any() ||
                                             apprenticeshipApplicationSummaries.Any(
                                                 a => statusesRequiringAction.Contains(a.Status)));
                return new ArchiveVacancyViewModel(hasOutstandingActions, vacancyId);
            }
        }
    }
}
