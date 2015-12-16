namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using System;
    using Common.Mediators;
    using Providers;
    using ViewModels.Application.Apprenticeship;

    public class ApprenticeshipApplicationMediator : MediatorBase, IApprenticeshipApplicationMediator
    {
        private readonly IApplicationProvider _applicationProvider;

        public ApprenticeshipApplicationMediator(IApplicationProvider applicationProvider)
        {
            _applicationProvider = applicationProvider;
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> GetApprenticeshipApplicationViewModel(Guid applicationId)
        {
            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationId);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.GetApprenticeshipApplicationViewModel.Ok, viewModel);
        }
    }
}