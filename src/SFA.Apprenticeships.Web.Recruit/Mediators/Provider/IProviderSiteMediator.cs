namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using Candidate.Mediators;
    using ViewModels.Provider;

    public interface IProviderSiteMediator
    {
        MediatorResponse<ProviderSiteSearchResponseViewModel> FindSite(ProviderSiteSearchViewModel searchViewModel);
    }
}
