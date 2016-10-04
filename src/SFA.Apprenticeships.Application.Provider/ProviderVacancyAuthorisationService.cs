namespace SFA.Apprenticeships.Application.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Vacancies;
    using Interfaces;
    using Interfaces.Providers;

    public class ProviderVacancyAuthorisationService : IProviderVacancyAuthorisationService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProviderService _providerService;

        public ProviderVacancyAuthorisationService(
            ICurrentUserService currentUserService,
            IProviderService providerService)
        {
            _currentUserService = currentUserService;
            _providerService = providerService;
        }

        public void Authorise(Vacancy vacancy)
        {
            if (!_currentUserService.IsInRole(Roles.Faa))
            {
                // Only Provider Users require authorisation (QA users are always authorised).
                return;
            }

            var ukprn = _currentUserService.GetClaimValue("ukprn");
            var ukprnoverride = _currentUserService.GetClaimValue("ukprnoverride");
            if (!string.IsNullOrEmpty(ukprnoverride))
            {
                ukprn = ukprnoverride;
            }
            else if (_currentUserService.IsInRole(Roles.Admin))
            {
                //This is to fix the anonymous view issue when impersonating
                return;
            }
            var provider = _providerService.GetProvider(ukprn);
            var vacancyId = vacancy.VacancyId;
            var contractOwnerId = vacancy.ProviderId;
            
            if (provider == null)
            {
                var message = $"Provider user '{_currentUserService.CurrentUserName}' signed in with invalid UKPRN '{ukprn}' attempted to view Vacancy Id '{vacancyId}' for Contract Owner Id '{contractOwnerId}', Vacancy Manager Id '{vacancy.VacancyManagerId}' and Delivery Organisation Id '{vacancy.DeliveryOrganisationId}'";

                throw new Domain.Entities.Exceptions.CustomException(
                    message, ErrorCodes.ProviderVacancyAuthorisation.InvalidUkprn);
            }

            if (provider.ProviderId == contractOwnerId)
            {
                return;
            }

            var providerSiteIds = new List<int?> {vacancy.VacancyManagerId, vacancy.DeliveryOrganisationId};

            // Fall back to Provider Site Id as the assigned provider for a vacancy could be a sub-contractor.
            foreach (var providerSiteId in providerSiteIds.Where(id => id.HasValue))
            {
                var providerSite = _providerService.GetProviderSite(providerSiteId.Value);

                if (providerSite != null && providerSite.ProviderSiteRelationships.Any(psr => psr.ProviderId == provider.ProviderId))
                {
                    return;
                }

                var providerSites = _providerService.GetProviderSites(ukprn);

                if (providerSites.Any(each => each.ProviderSiteId == providerSiteId))
                {
                    return;
                }
            }

            var errorMessage = $"Provider user '{_currentUserService.CurrentUserName}' (signed in as UKPRN '{ukprn}') attempted to view Vacancy Id '{vacancyId}' for Contract Owner Id '{contractOwnerId}', Vacancy Manager Id '{vacancy.VacancyManagerId}' and Delivery Organisation Id '{vacancy.DeliveryOrganisationId}'";

            throw new Domain.Entities.Exceptions.CustomException(
                errorMessage, ErrorCodes.ProviderVacancyAuthorisation.Failed);
        }
    }
}
