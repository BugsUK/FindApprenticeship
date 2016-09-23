namespace SFA.Apprenticeships.Web.Manage.Mediators.InformationRadiator
{
    using System;
    using Application.Interfaces;
    using Common.Mediators;
    using Domain.Raa.Interfaces.Reporting;
    using ViewModels;

    public class InformationRadiatorMediator : MediatorBase, IInformationRadiatorMediator
    {
        private readonly ILogService _logService;
        private readonly IReportingRepository _reportingRepo;

        public InformationRadiatorMediator(IReportingRepository reportingRepo, ILogService logService)
        {
            _reportingRepo = reportingRepo;
            _logService = logService;
        }

        public MediatorResponse<InformationRadiatorViewModel> GetInformationRadiatorViewModel()
        {
            var viewModel = new InformationRadiatorViewModel();

            try
            {
                var data = _reportingRepo.GetInformationRadiatorData();

                viewModel.TotalProviders = data.TotalProviders;
                viewModel.TotalProvidersAskedToOnboard = data.TotalProvidersAskedToOnboard;
                viewModel.TotalProvidersForcedToMigrate = data.TotalProvidersForcedToMigrate;
                viewModel.TotalProvidersOnboarded = data.TotalProvidersOnboarded;
                viewModel.TotalProvidersMigrated = data.TotalProvidersMigrated;
                viewModel.TotalProviderUserAccounts = data.TotalProviderUserAccounts;
                viewModel.TotalVacanciesCreatedViaRaa = data.TotalVacanciesCreatedViaRaa;
                viewModel.TotalDraftVacanciesCreatedViaRaa = data.TotalDraftVacanciesCreatedViaRaa;
                viewModel.TotalVacanciesInReviewViaRaa = data.TotalVacanciesInReviewViaRaa;
                viewModel.TotalVacanciesReferredViaRaa = data.TotalVacanciesReferredViaRaa;
                viewModel.TotalVacanciesApprovedViaRaa = data.TotalVacanciesApprovedViaRaa;
                viewModel.TotalVacanciesClosedViaRaa = data.TotalVacanciesClosedViaRaa;
                viewModel.TotalVacanciesArchivedViaRaa = data.TotalVacanciesArchivedViaRaa;
                viewModel.TotalApplicationsStartedForRaaVacancies = data.TotalApplicationsStartedForRaaVacancies;
                viewModel.TotalApplicationsSubmittedForRaaVacancies = data.TotalApplicationsSubmittedForRaaVacancies;
                viewModel.TotalUnsuccessfulApplicationsViaRaa = data.TotalUnsuccessfulApplicationsViaRaa;
                viewModel.TotalSuccessfulApplicationsViaRaa = data.TotalSuccessfulApplicationsViaRaa;

                return GetMediatorResponse(InformationRadiatorMediatorCodes.IndexCodes.Ok, viewModel);
            }
            catch (Exception ex)
            {
                _logService.Warn("Failed to get information radiator data", ex);

                return GetMediatorResponse(InformationRadiatorMediatorCodes.IndexCodes.Error, viewModel);
            }
        }
    }
}