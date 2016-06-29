namespace SFA.Apprenticeships.Application.Provider
{
    using System.Linq;
    using Domain.Entities.Raa;
    using Infrastructure.Interfaces;
    using Interfaces.Providers;

    public class ProviderVacancyAuthorisationService : IProviderVacancyAuthorisationService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProviderService _providerService;

        public ProviderVacancyAuthorisationService(ICurrentUserService currentUserService, IProviderService providerService)
        {
            _currentUserService = currentUserService;
            _providerService = providerService;
        }

        public void Authorise(int providerId, int? providerSiteId)
        {
            if (!_currentUserService.IsInRole(Roles.Faa))
            {
                // Only Provider Users require authorisation.
                return;
            }

            var ukprn = _currentUserService.GetClaimValue("ukprn");
            var provider = _providerService.GetProvider(ukprn);

            if (provider.ProviderId == providerId)
            {
                return;
            }

            /*
            // If simple Provider Id check fails, determine whether Provider is authorised to manage the vacancy using
            // Vacancy Owner Relationship.
            if (_providerService.IsProviderAuthorisedForVacancy(ukprn, vacancy.vacancyId))
            {
            }
            */

            // Fall back to Provider Site Id as the assigned provider for a vacancy could be a sub-contractor
            if (providerSiteId.HasValue)
            {
                var providerSites = _providerService.GetProviderSites(ukprn);

                if (providerSites.Any(each => each.ProviderSiteId == providerSiteId))
                {
                    return;
                }
            }

            var message = $"Provider user '{_currentUserService.CurrentUserName}' (signed in as UKPRN '{ukprn}') attempted to view vacancy for Provider Id '{providerId}' and Provider Site Id '{providerSiteId}'";

            throw new Domain.Entities.Exceptions.CustomException(message, Interfaces.ErrorCodes.ProviderVacancyAuthorisationFailed);
        }
    }
}
