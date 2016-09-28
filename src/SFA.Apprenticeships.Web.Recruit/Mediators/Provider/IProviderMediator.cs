namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Provider;

    public interface IProviderMediator
    {
        MediatorResponse<ProviderViewModel> Sites(string ukprn);
    }
}