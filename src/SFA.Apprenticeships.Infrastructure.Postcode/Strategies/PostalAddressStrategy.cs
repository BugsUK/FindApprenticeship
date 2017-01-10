namespace SFA.Apprenticeships.Infrastructure.Postcode.Strategies
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Application.Interfaces;
    using Configuration;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Locations;
    using Entities;
    using Newtonsoft.Json;
    using RestSharp;

    public class PostalAddressStrategy : IPostalAddressStrategy
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogService _logger;

        public PostalAddressStrategy(IConfigurationService configurationService, ILogService logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }

        public PostalAddress GetPostalAddresses(string companyName, string street, string addressLine1, string addressLine2, string addressLine3, string addressLine4, string town, string postcode)
        {
            //This code assumes that we don't particularly mind how accurate the address is, we simply want to geocode as best as possible and let the user deside if the address is correct
            var postalAddress = new PostalAddress
            {
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                AddressLine3 = addressLine3,
                AddressLine4 = addressLine4,
                AddressLine5 = null, //Currently always null
                Town = town,
                Postcode = postcode
            };
            
            //Geocode the addres. Must be successful
            var geoCodingServiceConfiguration = _configurationService.Get<GeoCodingServiceConfiguration>();
            var geocodingKey = geoCodingServiceConfiguration.Key;

            //http://www.pcapredict.com/Support/WebService/Geocoding/UK/Geocode/2.1/
            var geocodeClient = new RestClient("http://services.postcodeanywhere.co.uk/Geocoding/UK/Geocode/v2.10");

            //Initially try to geocode using postcode
            var geoPoints = GetGeoPoints(geocodeClient, $"json.ws?Key={geocodingKey}&Location={postcode}");

            if (!geoPoints.Any(gp => gp.IsSet()))
            {
                //If that fails, try full address
                geoPoints = GetGeoPoints(geocodeClient, $"json.ws?Key={geocodingKey}&Location={addressLine1},{addressLine2},{town}");

                if (!geoPoints.Any(gp => gp.IsSet()))
                {
                    //If that fails, try street and town
                    geoPoints = GetGeoPoints(geocodeClient, $"json.ws?Key={geocodingKey}&Location={street},{town}");

                    if (!geoPoints.Any(gp => gp.IsSet()))
                    {
                        //If that fails, try shortening the postcode until we get a hit
                        var geocodePostcode = postcode;
                        while (geocodePostcode.Length > 2 && geocodePostcode.Length > postcode.IndexOf(" ", StringComparison.InvariantCulture))
                        {
                            geocodePostcode = geocodePostcode.Substring(0, geocodePostcode.Length - 1);
                            geoPoints = GetGeoPoints(geocodeClient, $"json.ws?Key={geocodingKey}&Location={street},{town}");
                            if (geoPoints.Any(gp => gp.IsSet()))
                            {
                                break;
                            }
                        }
                    }
                }
            }

            var geopoint = geoPoints.FirstOrDefault(gp => gp.IsSet());
            if (geopoint == null)
            {
                var message = $"Failed to geocode address {postalAddress}";
                _logger.Error(message);
                throw new CustomException(message, Postcode.ErrorCodes.PostalAddressGeocodeFailed);
            }
            postalAddress.GeoPoint = geopoint;

            //We have a geocode. Now get local authority and county
            //http://www.pcapredict.com/Support/WebService/GovernmentData/Postzon/RetrieveByCoordinates/1.2/
            var postzonRetrieveByCoordinatesClient = new RestClient("http://services.postcodeanywhere.co.uk/GovernmentData/Postzon/RetrieveByCoordinates/v1.20");
            var postzons = GetPostzon(postzonRetrieveByCoordinatesClient, $"json.ws?Key={geocodingKey}&CentrePoint={geopoint.Latitude},{geopoint.Longitude}");

            return postalAddress;
        }

        private GeoPoint[] GetGeoPoints(IRestClient client, string resource)
        {
            //TODO: async
            //Retry up to three times with backoff
            var errorStringBuilder = new StringBuilder("Failed to retrieve geocode due to network error. The following errors occured:\r\n");
            for (var i = 0; i < 3; i++)
            {
                var request = new RestRequest(resource)
                {
                    RequestFormat = DataFormat.Json
                };

                var response = client.Execute(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    var geoPoints = JsonConvert.DeserializeObject<GeoPoint[]>(response.Content);
                    return geoPoints;
                }
                errorStringBuilder.AppendLine($"Request {i + 1}. ResponseStatus: {response.ResponseStatus} StatusCode {response.StatusCode}");
                Thread.Sleep(1000 * i);
            }

            var message = errorStringBuilder.ToString();
            _logger.Error(message);
            throw new CustomException(message, Postcode.ErrorCodes.PostalAddressGeocodeRequestFailed);
        }

        private Postzon[] GetPostzon(IRestClient client, string resource)
        {
            //TODO: async
            //Retry up to three times with backoff
            var errorStringBuilder = new StringBuilder("Failed to retrieve local authority and county due to network error. The following errors occured:\r\n");
            for (var i = 0; i < 3; i++)
            {
                var request = new RestRequest(resource)
                {
                    RequestFormat = DataFormat.Json
                };

                var response = client.Execute(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    var postzons = JsonConvert.DeserializeObject<Postzon[]>(response.Content);
                    return postzons;
                }
                errorStringBuilder.AppendLine($"Request {i + 1}. ResponseStatus: {response.ResponseStatus} StatusCode {response.StatusCode}");
                Thread.Sleep(1000 * i);
            }

            var message = errorStringBuilder.ToString();
            _logger.Warn(message);
            throw new CustomException(message, Postcode.ErrorCodes.PostalAddressLocalAuthorityCountyRequestFailed);
        }
    }
}