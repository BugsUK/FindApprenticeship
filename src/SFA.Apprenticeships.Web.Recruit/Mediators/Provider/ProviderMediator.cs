namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.Providers;

    public class ProviderMediator : MediatorBase, IProviderMediator
    {
        private readonly IProviderProvider _providerProvider;

        public ProviderMediator(IProviderProvider providerProvider)
        {
            _providerProvider = providerProvider;
        }

        public MediatorResponse<ProviderViewModel> Sites(string ukprn)
        {
            var providerProfile = _providerProvider.GetProviderViewModel(ukprn);
            return GetMediatorResponse(ProviderMediatorCodes.Sites.Ok, providerProfile);
        }
    }
}