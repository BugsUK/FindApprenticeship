namespace SFA.Apprenticeships.Web.Manage.Mediators.InformationRadiator
{
    using Common.Mediators;
    using Domain.Raa.Interfaces.Reporting;
    using ViewModels;

    public class InformationRadiatorMediator : MediatorBase, IInformationRadiatorMediator
    {
        private readonly IReportingRepository _reportingRepo;

        public InformationRadiatorMediator(IReportingRepository reportingRepo)
        {
            _reportingRepo = reportingRepo;
        }

        public MediatorResponse<InformationRadiatorViewModel> GetInformationRadiatorViewModel()
        {
            var data = _reportingRepo.GetInformationRadiatorData();

            var viewModel = new InformationRadiatorViewModel
            {
                TotalProviders = data.TotalProviders,
                TotalProviderUserAccounts = data.TotalProviderUserAccounts,
                TotalVacanciesApprovedViaRaa = data.TotalVacanciesApprovedViaRaa,
                TotalVacanciesSubmittedViaRaa = data.TotalVacanciesSubmittedViaRaa,
                TotalApplicationsSubmittedForRaaVacancies = data.TotalApplicationsSubmittedForRaaVacancies,
                TotalUnsuccessfulApplicationsViaRaa = data.TotalUnsuccessfulApplicationsViaRaa,
                TotalSuccessfulApplicationsViaRaa = data.TotalSuccessfulApplicationsViaRaa
            };

            return GetMediatorResponse(InformationRadiatorMediatorCodes.IndexCodes.Ok, viewModel);
        }
    }
}