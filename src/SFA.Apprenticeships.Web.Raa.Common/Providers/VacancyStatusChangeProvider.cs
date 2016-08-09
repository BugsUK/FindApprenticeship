namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Applications;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using ViewModels.Vacancy;
    using ViewModels.VacancyStatus;

    public class VacancyStatusChangeProvider : IVacancyStatusChangeProvider
    {
        private readonly IApprenticeshipApplicationService _apprenticeshipApplicationService;
        private readonly ITraineeshipApplicationService _traineeshipApplicationService;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IVacancyPostingService _vacancyPostingService;

        public VacancyStatusChangeProvider(IApprenticeshipApplicationService apprenticeshipApplicationService,
            ITraineeshipApplicationService traineeshipApplicationService,
            IVacancyReadRepository vacancyReadRepository,
            IVacancyPostingService vacancyPostingService)
        {
            _apprenticeshipApplicationService = apprenticeshipApplicationService;
            _traineeshipApplicationService = traineeshipApplicationService;
            _vacancyReadRepository = vacancyReadRepository;
            _vacancyPostingService = vacancyPostingService;
        }

        public ArchiveVacancyViewModel GetArchiveVacancyViewModelByVacancyReferenceNumber(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyReadRepository.GetByReferenceNumber(vacancyReferenceNumber);
            return new ArchiveVacancyViewModel(HasOutstandingActions(vacancy), vacancy.VacancyId, vacancyReferenceNumber);
        }

        private bool HasOutstandingActions(Vacancy vacancy)
        {
            //TODO: Put this somewhwere more common
            var statusesRequiringAction = new List<ApplicationStatuses>()
            {
                ApplicationStatuses.Submitting,
                ApplicationStatuses.InProgress,
                ApplicationStatuses.Submitted
            };

            if (vacancy.VacancyType == VacancyType.Traineeship)
            {
                var traineeshipApplicationSummaries =
                    _traineeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId);
                return (traineeshipApplicationSummaries.Any(a => statusesRequiringAction.Contains(a.Status)));
            }
            var apprenticeshipApplicationSummaries = _apprenticeshipApplicationService.GetApplicationSummaries(vacancy.VacancyId);
            return (apprenticeshipApplicationSummaries.Any(a => statusesRequiringAction.Contains(a.Status)));
        }

        public ArchiveVacancyViewModel ArchiveVacancy(ArchiveVacancyViewModel viewModel)
        {
            var vacancy = _vacancyReadRepository.GetByReferenceNumber(viewModel.VacancyReferenceNumber);

            //Ensure this vacancy has no outstanding actions
            var hasOustandingActions = HasOutstandingActions(vacancy);

            if (!hasOustandingActions)
            {
                vacancy = _vacancyPostingService.ArchiveVacancy(vacancy);
            }

            return new ArchiveVacancyViewModel(hasOustandingActions, vacancy.VacancyId, vacancy.VacancyReferenceNumber);
        }
    }
}
