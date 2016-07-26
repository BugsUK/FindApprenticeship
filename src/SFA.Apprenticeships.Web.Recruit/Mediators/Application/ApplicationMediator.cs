namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Common.Mediators;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Application;

    public class ApplicationMediator : MediatorBase, IApplicationMediator
    {
        private readonly IApplicationProvider _applicationProvider;

        public ApplicationMediator(IApplicationProvider applicationProvider)
        {
            _applicationProvider = applicationProvider;
        }

        public MediatorResponse<VacancyApplicationsViewModel> GetVacancyApplicationsViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearch)
        {
            var viewModel = _applicationProvider.GetVacancyApplicationsViewModel(vacancyApplicationsSearch);

            return GetMediatorResponse(ApplicationMediatorCodes.GetVacancyApplicationsViewModel.Ok, viewModel);
        }

        public MediatorResponse<ShareApplicationsViewModel> ShareApplications(int vacancyReferenceNumber)
        {
            var viewModel = _applicationProvider.GetShareApplicationsViewModel(vacancyReferenceNumber);

            if (viewModel.ApplicationSummaries.Count == 0)
            {
                return GetMediatorResponse(ApplicationMediatorCodes.GetShareApplicationsViewModel.NoApplications, viewModel);
            }

            return GetMediatorResponse(ApplicationMediatorCodes.GetShareApplicationsViewModel.Ok, viewModel);
        }
    }
}