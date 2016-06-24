namespace SFA.Apprenticeships.Application.Interfaces.Providers
{
    public interface IProviderVacancyAuthorisationService
    {
        void Authorise(int providerId, int? providerSiteId);
    }
}
