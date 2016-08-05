namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Applications;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories;
    using ViewModels.VacancyStatus;

    public class VacancyStatusChangeProvider : IVacancyStatusChangeProvider
    {
        private readonly IApprenticeshipApplicationService _apprenticeshipApplicationService;
        private readonly ITraineeshipApplicationService _traineeshipApplicationService;
        private readonly IVacancyReadRepository _vacancyReadRepository;

        public VacancyStatusChangeProvider(IApprenticeshipApplicationService apprenticeshipApplicationService, ITraineeshipApplicationService traineeshipApplicationService, IVacancyReadRepository vacancyReadRepository)
        {
            _apprenticeshipApplicationService = apprenticeshipApplicationService;
            _traineeshipApplicationService = traineeshipApplicationService;
            _vacancyReadRepository = vacancyReadRepository;
        }

        public ArchiveVacancyViewModel GetArchiveVacancyViewModelByVacancyReferenceNumber(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyReadRepository.GetByReferenceNumber(vacancyReferenceNumber);
            
            //TODO: Put this somewhwere more common
            var statusesRequiringAction = new List<ApplicationStatuses>()
            {
                ApplicationStatuses.Submitting,
                ApplicationStatuses.InProgress,
                ApplicationStatuses.Submitted
            };

            if (vacancy.VacancyType == VacancyType.Traineeship)
            {
                var traineeshipApplicationSummaries = _traineeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId);
                var hasOutstandingActions = (!traineeshipApplicationSummaries.Any() ||
                                             traineeshipApplicationSummaries.Any(
                                                 a => statusesRequiringAction.Contains(a.Status)));
                return new ArchiveVacancyViewModel(hasOutstandingActions, vacancy.VacancyId, vacancyReferenceNumber);

            }
            else
            {
                var apprenticeshipApplicationSummaries = _apprenticeshipApplicationService.GetApplicationSummaries(vacancy.VacancyId);
                var hasOutstandingActions = (!apprenticeshipApplicationSummaries.Any() ||
                                             apprenticeshipApplicationSummaries.Any(
                                                 a => statusesRequiringAction.Contains(a.Status)));
                return new ArchiveVacancyViewModel(hasOutstandingActions, vacancy.VacancyId, vacancyReferenceNumber);
            }
        }
    }
}
