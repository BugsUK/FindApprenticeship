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

        public MediatorResponse<VacancyApplicationsViewModel> GetVacancyApplicationsViewModel(long vacancyReferenceNumber)
        {
            var viewModel = _applicationProvider.GetVacancyApplicationsViewModel(vacancyReferenceNumber);

            return GetMediatorResponse(ApplicationMediatorCodes.GetVacancyApplicationsViewModel.Ok, viewModel);
        }
    }
}