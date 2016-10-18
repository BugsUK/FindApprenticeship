namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyStatus
{
    using Apprenticeships.Application.Interfaces;
    using Common.Mediators;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.VacancyStatus;
    using VacancyPosting;

    public class VacancyStatusMediator : IVacancyStatusMediator
    {
        private readonly IVacancyStatusChangeProvider _vacancyStatusChangeProvider;
        private readonly IApplicationProvider _applicationProvider;
        private readonly IMapper _mapper;

        public VacancyStatusMediator(IVacancyStatusChangeProvider vacancyStatusChangeProvider, IMapper mapper, IApplicationProvider applicationProvider)
        {
            _vacancyStatusChangeProvider = vacancyStatusChangeProvider;
            _mapper = mapper;
            _applicationProvider = applicationProvider;
        }

        public MediatorResponse<ArchiveVacancyViewModel> GetArchiveVacancyViewModelByVacancyReferenceNumber(int vacancyReferenceNumber)
        {
            var model = _vacancyStatusChangeProvider.GetArchiveVacancyViewModelByVacancyReferenceNumber(vacancyReferenceNumber);
            return new MediatorResponse<ArchiveVacancyViewModel>()
            {
                ViewModel = model,
                Code = VacancyStatusMediatorCodes.GetArchiveVacancyViewModel.Ok
            };
        }

        public MediatorResponse<ArchiveVacancyViewModel> ArchiveVacancy(ArchiveVacancyViewModel viewModel)
        {
            var model = _vacancyStatusChangeProvider.ArchiveVacancy(viewModel);

            if (model.HasOutstandingActions)
            {
                return new MediatorResponse<ArchiveVacancyViewModel>()
                {
                    ViewModel = model,
                    Code = VacancyStatusMediatorCodes.ArchiveVacancy.OutstandingActions
                };
            }

            return new MediatorResponse<ArchiveVacancyViewModel>
            {
                ViewModel = model,
                Code = VacancyStatusMediatorCodes.ArchiveVacancy.Ok
            };
        }

        public MediatorResponse<BulkDeclineCandidatesViewModel> GetBulkDeclineCandidatesViewModelByVacancyReferenceNumber(int vacancyReferenceNumber)
        {
            var model = _applicationProvider.GetBulkDeclineCandidatesViewModel(vacancyReferenceNumber);
            return new MediatorResponse<BulkDeclineCandidatesViewModel>
            {
                ViewModel = model,
                Code = VacancyStatusMediatorCodes.BulkDeclineCandidatesViewModel.Ok,
            };
        }

        public MediatorResponse<BulkDeclineCandidatesViewModel> GetBulkDeclineCandidatesViewModelByVacancyReferenceNumber(BulkDeclineCandidatesViewModel viewModel)
        {
            var model = _vacancyStatusChangeProvider.BulkDeclineCandidates(viewModel);

            if (model.CanBulkDeclineCandidates)
            {
                return new MediatorResponse<BulkDeclineCandidatesViewModel>()
                {
                    ViewModel = model,
                    Code = VacancyStatusMediatorCodes.BulkDeclineCandidatesViewModel.OutstandingActions
                };
            }

            return new MediatorResponse<BulkDeclineCandidatesViewModel>()
            {
                ViewModel = model,
                Code = VacancyStatusMediatorCodes.BulkDeclineCandidatesViewModel.Ok
            };
        }
    }
}