namespace SFA.Apprenticeships.Application.Services.LocationSearchService
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.AppSettings;
    using Domain.Entities;
    using Entities;
    using Extensions;
    using Interfaces;
    using RestSharp;

    public class LocationSearchService : ILocationSearchService 
    {
        private const string DatasetDpa = "DPA";
        private const string DatasetLpi = "LPI";

        public IEnumerable<Location> FindAddress(string postcode)
        {
            var restClient = new RestClient("https://api.ordnancesurvey.co.uk/places/v1/addresses/postcode");

            var restRequest = new RestRequest(Method.GET);

            var dataset = DatasetDpa;
            //var dataset = DatasetLpi;

            restRequest.AddParameter("postcode", postcode);
            restRequest.AddParameter("dataset", dataset);
            restRequest.AddParameter("format", "json");
            restRequest.AddParameter("key", BaseAppSettingValues.OrdnanceSurveyPlacesApiKey);

            var restResponse = restClient.Execute<OrdnanceSurveyPlacesResponse>(restRequest);
            if (restResponse.ResponseStatus == ResponseStatus.Completed)
            {
                var results = restResponse.Data.Results;

                if (results != null)
                {
                    if (dataset == DatasetDpa)
                    {
                        return results
                            .Select(r => r.DeliveryPointAddress.ToLocation())
                            .OrderBy(a => a.Address.AddressLine1);
                    }

                    if (dataset == DatasetLpi)
                    {
                        return results
                            .Where(
                                r =>
                                    r.LocalPropertyIdentifier.IsResidential() ||
                                    r.LocalPropertyIdentifier.IsCommercial())
                            .Select(r => r.LocalPropertyIdentifier.ToLocation())
                            .OrderBy(a => a.Address.AddressLine1);
                    }
                }
            }

            return Enumerable.Empty<Location>();
        }
    }
}