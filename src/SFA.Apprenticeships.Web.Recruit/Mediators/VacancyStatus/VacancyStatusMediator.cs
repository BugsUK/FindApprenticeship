namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyStatus
{
    using Common.Constants;
    using Common.Mediators;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.Validators.VacancyStatus;
    using Raa.Common.ViewModels.Application.Apprenticeship;
    using Raa.Common.ViewModels.VacancyStatus;
    using VacancyPosting;

    public class VacancyStatusMediator : MediatorBase, IVacancyStatusMediator
    {
        private readonly IVacancyStatusChangeProvider _vacancyStatusChangeProvider;
        private readonly IApplicationProvider _applicationProvider;
        private readonly BulkApplicationsRejectViewModelServerValidator _bulkApplicationsRejectViewModelServerValidator;

        public VacancyStatusMediator(IVacancyStatusChangeProvider vacancyStatusChangeProvider,
            IApplicationProvider applicationProvider,
            BulkApplicationsRejectViewModelServerValidator bulkApplicationsRejectViewModelServerValidator)
        {
            _vacancyStatusChangeProvider = vacancyStatusChangeProvider;
            _applicationProvider = applicationProvider;
            _bulkApplicationsRejectViewModelServerValidator = bulkApplicationsRejectViewModelServerValidator;
        }

        public MediatorResponse<ArchiveVacancyViewModel> GetArchiveVacancyViewModelByVacancyReferenceNumber(int vacancyReferenceNumber)
        {
            var model = _vacancyStatusChangeProvider.GetArchiveVacancyViewModelByVacancyReferenceNumber(vacancyReferenceNumber);
            return new MediatorResponse<ArchiveVacancyViewModel>
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

        public MediatorResponse<BulkDeclineCandidatesViewModel> BulkResponseApplications(BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel)
        {
            var viewModel = _applicationProvider.GetBulkDeclineCandidatesViewModel(bulkApplicationsRejectViewModel.VacancyReferenceNumber);
            var validationResult = _bulkApplicationsRejectViewModelServerValidator.Validate(bulkApplicationsRejectViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyStatusMediatorCodes.BulkApplicationsReject.FailedValidation, viewModel, validationResult);
            }
            return GetMediatorResponse(VacancyStatusMediatorCodes.BulkApplicationsReject.Ok, viewModel, EmployerSearchViewModelMessages.ErnAdviceText, UserMessageLevel.Info);
        }
    }
}