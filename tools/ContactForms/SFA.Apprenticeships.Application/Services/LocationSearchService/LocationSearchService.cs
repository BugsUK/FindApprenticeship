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
        public IEnumerable<Location> FindAddress(string postcode)
        {
            var restClient = new RestClient("https://api.ordnancesurvey.co.uk/places/v1/addresses/postcode");

            var restRequest = new RestRequest(Method.GET);

            restRequest.AddParameter("postcode", postcode);
            restRequest.AddParameter("dataset", "DPA");
            restRequest.AddParameter("format", "json");
            restRequest.AddParameter("key", BaseAppSettingValues.OrdnanceSurveyPlacesApiKey);

            var results = restClient.Execute<OrdnanceSurveyPlacesResponse>(restRequest).Data.Results;

            return results
                    //.Where(r => r.LocalPropertyIdentifier.IsResidential())
                    .Select(r => r.DeliveryPointAddress.ToLocation())
                    .OrderBy(l => l.Address.AddressLine1);
        }
    }
}