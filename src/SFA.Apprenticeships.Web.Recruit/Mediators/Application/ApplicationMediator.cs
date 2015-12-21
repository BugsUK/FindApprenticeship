namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Common.Mediators;
    using Providers;
    using ViewModels.Application;

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
    }
}