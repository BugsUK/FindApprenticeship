namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using Application.Interfaces;
    using DAS.RAA.Api.Client.V1;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.RaaApi;
    using Domain.Raa.Interfaces.Repositories;
    using Microsoft.Rest;
    using Web.Common.Configuration;

    public class ApiClientProvider : IApiClientProvider
    {
        private readonly IConfigurationService _configurationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRaaApiUserRepository _raaApiUserRepository;
        private readonly ILogService _logService;

        public ApiClientProvider(IConfigurationService configurationService, ICurrentUserService currentUserService, IRaaApiUserRepository raaApiUserRepository, ILogService logService)
        {
            _configurationService = configurationService;
            _currentUserService = currentUserService;
            _raaApiUserRepository = raaApiUserRepository;
            _logService = logService;
        }

        public IApiClient GetApiClient()
        {
            var baseUrl = _configurationService.Get<CommonWebConfiguration>().RaaApiBaseUrl;
            var apiKey = _currentUserService.GetClaimValue(ClaimTypes.RaaApiKey);
            if (string.IsNullOrEmpty(apiKey))
            {
                var ukprn = _currentUserService.GetClaimValue(ClaimTypes.Ukprn);
                var ukprnoverride = _currentUserService.GetClaimValue(ClaimTypes.UkprnOverride);
                if (!string.IsNullOrEmpty(ukprnoverride))
                {
                    ukprn = ukprnoverride;
                }
                var apiUser = _raaApiUserRepository.GetUser(Convert.ToInt32(ukprn));
                if (apiUser == null || ReferenceEquals(apiUser, RaaApiUser.UnknownApiUser))
                {
                    var message = $"No RAA API key found for current principal {_currentUserService.CurrentUserName}";
                    _logService.Error(message);
                    throw new CustomException(message);
                }
                apiKey = apiUser.PrimaryApiKey.ToString();

            }
            var apiClient = new ApiClient(new Uri(baseUrl), new TokenCredentials(apiKey, "bearer"));
            return apiClient;
        }
    }
}