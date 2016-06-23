namespace SFA.Apprenticeships.Application.Provider
{
    using System;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa;
    using Infrastructure.Interfaces;
    using Interfaces.Providers;

    public class ProviderVacancyAuthorisationService : IProviderVacancyAuthorisationService
    {
        private readonly ICurrentUserService _currentUserService;

        public ProviderVacancyAuthorisationService(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public void Authorise(int providerId)
        {
            if (!_currentUserService.IsInRole(Roles.Faa))
            {
                // Only Provider Users require authorisation.
                return;
            }

            var signedInProviderId = _currentUserService.GetClaimValue("providerId");

            if (Convert.ToString(providerId) == signedInProviderId)
            {
                return;
            }

            var message = $"Provider user '{_currentUserService.CurrentUserName}' (signed in as Provider Id {signedInProviderId}) attempted to view vacancy for Provider Id {providerId}";

            throw new CustomException(message, Interfaces.ErrorCodes.ProviderVacancyAuthorisationFailed);
        }
    }
}
