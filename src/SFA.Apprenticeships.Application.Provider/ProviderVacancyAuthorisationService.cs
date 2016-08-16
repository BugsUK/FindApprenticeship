namespace SFA.Apprenticeships.Application.Provider
{
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
            var provider = _providerService.GetProvider(ukprn);
            var vacancyId = vacancy.VacancyId;
            var providerId = vacancy.ProviderId;
            var providerSiteId = vacancy.VacancyManagerId;

            if (provider == null)
            {
                var message = $"Provider user '{_currentUserService.CurrentUserName}' signed in with invalid UKPRN '{ukprn}' attempted to view Vacancy Id '{vacancyId}' for Provider Id '{providerId}' and Provider Site Id '{providerSiteId}'";

                throw new Domain.Entities.Exceptions.CustomException(
                    message, Interfaces.ErrorCodes.ProviderVacancyAuthorisation.InvalidUkprn);
            }

            if (provider.ProviderId == providerId)
            {
                return;
            }

            // Fall back to Provider Site Id as the assigned provider for a vacancy could be a sub-contractor.
            if (providerSiteId.HasValue)
            {
                var providerSites = _providerService.GetProviderSites(ukprn);

                if (providerSites.Any(each => each.ProviderSiteId == providerSiteId))
                {
                    return;
                }
            }

            {
                var message = $"Provider user '{_currentUserService.CurrentUserName}' (signed in as UKPRN '{ukprn}') attempted to view Vacancy Id '{vacancyId}' for Provider Id '{providerId}' and Provider Site Id '{providerSiteId}'";

                throw new Domain.Entities.Exceptions.CustomException(
                    message, Interfaces.ErrorCodes.ProviderVacancyAuthorisation.Failed);
            }
        }
    }
}
