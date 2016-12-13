namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using Application.Interfaces;
    using DAS.RAA.Api.Client.V1;
    using Microsoft.Rest;

    public class ApiClientProvider : IApiClientProvider
    {
        private readonly IConfigurationService _configurationService;

        public ApiClientProvider(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public IApiClient GetApiClient()
        {
            //TODO: Get the url from config and the token credentials from the IPrincipal
            var apiClient = new ApiClient(new Uri("http://local-restapi.findapprenticeship.service.gov.uk/"), new TokenCredentials("D359D967-162C-4E53-8389-EBF7B9B69619", "bearer"));
            return apiClient;
        }
    }
}