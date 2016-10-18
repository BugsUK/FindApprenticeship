namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces.Applications;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.VacancyStatus;

    public class VacancyStatusChangeProvider : IVacancyStatusChangeProvider
    {
        private readonly IApprenticeshipApplicationService _apprenticeshipApplicationService;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IVacancyPostingService _vacancyPostingService;

        public VacancyStatusChangeProvider(IApprenticeshipApplicationService apprenticeshipApplicationService,
            IVacancyReadRepository vacancyReadRepository,
            IVacancyPostingService vacancyPostingService)
        {
            _apprenticeshipApplicationService = apprenticeshipApplicationService;
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

            var apprenticeshipApplicationSummaries = _apprenticeshipApplicationService.GetApplicationSummaries(vacancy.VacancyId);

            return (apprenticeshipApplicationSummaries.Any(a => statusesRequiringAction.Contains(a.Status)));
        }        

        public ArchiveVacancyViewModel ArchiveVacancy(ArchiveVacancyViewModel viewModel)
        {
            var vacancy = _vacancyReadRepository.GetByReferenceNumber(viewModel.VacancyReferenceNumber);

            if (vacancy.VacancyType == VacancyType.Traineeship)
            {
                throw new InvalidOperationException("Traineeships cannot be archived");
            }

            var hasOustandingActions = HasOutstandingActions(vacancy);

            if (!hasOustandingActions)
            {
                vacancy = _vacancyPostingService.ArchiveVacancy(vacancy);
            }

            return new ArchiveVacancyViewModel(hasOustandingActions, vacancy.VacancyId, vacancy.VacancyReferenceNumber);
        }

        public BulkDeclineCandidatesViewModel BulkDeclineCandidates(BulkDeclineCandidatesViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
