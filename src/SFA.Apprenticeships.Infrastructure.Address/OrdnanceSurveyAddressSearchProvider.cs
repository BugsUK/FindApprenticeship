namespace SFA.Apprenticeships.Infrastructure.Address
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Application.Address;
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Configuration;
    using Entities;
    using Extensions;
    using RestSharp;

    public class OrdnanceSurveyAddressSearchProvider : IAddressSearchProvider
    {
        private const string DatasetDpa = "DPA";
        private const string DatasetLpi = "LPI";

        private readonly ILogService _logService;
        private readonly IRestClient _restClient;
        private readonly OrdnanceSurveyPlacesConfiguration _configuration;

        public OrdnanceSurveyAddressSearchProvider(
            ILogService logService,
            IConfigurationService configurationService,
            IRestClient restClient)
        {
            _logService = logService;
            _restClient = restClient;
            _configuration = configurationService.Get<AddressSearchConfiguration>().OrdnanceSurveyPlacesConfiguration;
        }

        public IEnumerable<Address> FindAddress(string postcode)
        {
            _restClient.BaseUrl = new Uri(_configuration.BaseUrl);

            var restRequest = new RestRequest(Method.GET);

            restRequest.AddParameter("postcode", postcode);
            restRequest.AddParameter("dataset", _configuration.Dataset);
            restRequest.AddParameter("format", "json");
            restRequest.AddParameter("key", _configuration.ApiKey);

            var restResponse = _restClient.Execute<OrdnanceSurveyPlacesResponse>(restRequest);

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                var results = restResponse.Data.Results;

                if (results != null)
                {
                    if (_configuration.Dataset == DatasetDpa)
                    {
                        return results
                            .Where(r => r.DeliveryPointAddress.IsResidential())
                            .Select(r => r.DeliveryPointAddress.ToAddress())
                            .OrderBy(a => a.AddressLine1);
                    }

                    if (_configuration.Dataset == DatasetLpi)
                    {
                        return results
                            .Where(r => r.LocalPropertyIdentifier.IsResidential())
                            .Select(r => r.LocalPropertyIdentifier.ToAddress())
                            .OrderBy(a => a.AddressLine1);
                    }
                }
            }
            else
            {
                _logService.Warn("Find address failed with status code: {0} and description '{1}', content '{2}'",
                    restResponse.StatusCode, restResponse.StatusDescription, restResponse.Content);
            }

            return Enumerable.Empty<Address>();
        }
    }
}