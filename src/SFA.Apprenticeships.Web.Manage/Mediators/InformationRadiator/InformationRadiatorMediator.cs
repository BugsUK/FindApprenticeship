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
                viewModel.TotalProvidersMigrated = data.TotalProvidersMigrated;
                viewModel.TotalVacanciesCreatedViaRaa = data.TotalVacanciesCreatedViaRaa;
                viewModel.TotalVacanciesSubmittedViaRaa = data.TotalVacanciesSubmittedViaRaa;
                viewModel.TotalVacanciesApprovedViaRaa = data.TotalVacanciesApprovedViaRaa;
                viewModel.TotalVacanciesReferredViaRaa = data.TotalVacanciesReferredViaRaa;
                viewModel.TotalVacanciesInReviewViaRaa = data.TotalVacanciesInReviewViaRaa;
                viewModel.VacanciesSubmittedToday = data.VacanciesSubmittedToday;
                viewModel.VacanciesSubmittedYesterday = data.VacanciesSubmittedYesterday;
                viewModel.VacanciesSubmittedTwoDaysAgo = data.VacanciesSubmittedTwoDaysAgo;
                viewModel.VacanciesSubmittedThreeDaysAgo = data.VacanciesSubmittedThreeDaysAgo;
                viewModel.VacanciesSubmittedFourDaysAgo = data.VacanciesSubmittedFourDaysAgo;
                viewModel.VacanciesReviewedToday = data.VacanciesReviewedToday;
                viewModel.VacanciesReviewedYesterday = data.VacanciesReviewedYesterday;
                viewModel.VacanciesReviewedTwoDaysAgo = data.VacanciesReviewedTwoDaysAgo;
                viewModel.VacanciesReviewedThreeDaysAgo = data.VacanciesReviewedThreeDaysAgo;
                viewModel.VacanciesReviewedFourDaysAgo = data.VacanciesReviewedFourDaysAgo;
                viewModel.TotalApplicationsStartedInPastFourWeeks = data.TotalApplicationsStartedInPastFourWeeks;
                viewModel.TotalApplicationsSubmittedInPastFourWeeks = data.TotalApplicationsSubmittedInPastFourWeeks;
                viewModel.TotalUnsuccessfulApplicationsInPastFourWeeks = data.TotalUnsuccessfulApplicationsInPastFourWeeks;
                viewModel.TotalSuccessfulApplicationsInPastFourWeeks = data.TotalSuccessfulApplicationsInPastFourWeeks;

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