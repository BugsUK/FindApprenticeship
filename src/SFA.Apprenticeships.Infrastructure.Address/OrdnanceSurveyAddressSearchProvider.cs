namespace SFA.Apprenticeships.Infrastructure.Address
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Address;
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

        private readonly IRestClient _restClient;
        private readonly OrdnanceSurveyPlacesConfiguration _configuration;

        public OrdnanceSurveyAddressSearchProvider(IConfigurationService configurationService, IRestClient restClient)
        {
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

            var results = _restClient.Execute<OrdnanceSurveyPlacesResponse>(restRequest).Data.Results;

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

            return Enumerable.Empty<Address>();
        }
    }
}