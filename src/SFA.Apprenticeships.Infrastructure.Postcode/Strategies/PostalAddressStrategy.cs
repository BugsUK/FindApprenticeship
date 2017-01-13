namespace SFA.Apprenticeships.Infrastructure.Postcode.Strategies
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Application.Interfaces;
    using Application.Location.Strategies;
    using Configuration;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Locations;
    using Entities;
    using Newtonsoft.Json;
    using RestSharp;
    using System.Collections.Generic;
    using Application.Interfaces.ReferenceData;

    public class PostalAddressStrategy : IPostalAddressStrategy
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly IConfigurationService _configurationService;
        private readonly ILogService _logger;

        public PostalAddressStrategy(IReferenceDataService referenceDataService, IConfigurationService configurationService, ILogService logger)
        {
            _referenceDataService = referenceDataService;
            _configurationService = configurationService;
            _logger = logger;
        }

        public PostalAddress GetPostalAddresses(string companyName, string primaryAddressableObject, string secondaryAddressableObject, string street, string town, string postcode)
        {
            //This code assumes that we don't particularly mind how accurate the address is, we simply want to geocode as best as possible and let the user deside if the address is correct
            var postalAddress = new PostalAddress
            {
                AddressLine1 = GetAddressLine1(primaryAddressableObject, secondaryAddressableObject, street),
                AddressLine2 = GetAddressLine2(primaryAddressableObject, secondaryAddressableObject, street),
                AddressLine3 = GetAddressLine3(primaryAddressableObject, secondaryAddressableObject, street),
                AddressLine4 = "", //Used to be town, Currently always empty string
                AddressLine5 = null, //Currently always null
                Town = town,
                Postcode = postcode
            };

            //Try and get the address itself from PCA. Can fail
            var postalAddressServiceConfiguration = _configurationService.Get<PostalAddressServiceConfiguration>();
            var addressKey = postalAddressServiceConfiguration.Key;

            //http://www.pcapredict.com/support/webservice/postcodeanywhere/interactive/retrievebyaddress/1.2/
            var addressClient = new RestClient("http://services.postcodeanywhere.co.uk/PostcodeAnywhere/Interactive/RetrieveByAddress/v1.20");
            var addresses = GetAddresses(addressClient, $"json.ws?Key={addressKey}&Address={string.Join(",", GetAddressComponents(postalAddress))},{postcode}");
            var address = addresses.FirstOrDefault();
            if (address != null)
            {
                postalAddress.AddressLine1 = address.Line1;
                postalAddress.AddressLine2 = address.Line2;
                postalAddress.AddressLine3 = address.Line3;
                postalAddress.AddressLine4 = address.Line4;
                postalAddress.AddressLine5 = address.Line5;
                postalAddress.Town = address.PostTown;
                postalAddress.Postcode = address.Postcode;
                postalAddress.ValidationSourceKeyValue = address.Udprn.ToString();
                postalAddress.ValidationSourceCode = "PCA";
            }

            //Geocode the address. Must be successful
            var geoCodingServiceConfiguration = _configurationService.Get<GeoCodingServiceConfiguration>();
            var geocodingKey = geoCodingServiceConfiguration.Key;

            //http://www.pcapredict.com/Support/WebService/Geocoding/UK/Geocode/2.1/
            var geocodeClient = new RestClient("http://services.postcodeanywhere.co.uk/Geocoding/UK/Geocode/v2.10");

            //Initially try to geocode using postcode
            var geoPoints = GetGeoPoints(geocodeClient, $"json.ws?Key={geocodingKey}&Location={postcode}");

            if (!geoPoints.Any(gp => gp.IsSet()))
            {
                //If that fails, try full address
                geoPoints = GetGeoPoints(geocodeClient, $"json.ws?Key={geocodingKey}&Location={string.Join(",", GetAddressComponents(postalAddress))}");

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
                            geoPoints = GetGeoPoints(geocodeClient, $"json.ws?Key={geocodingKey}&Location={geocodePostcode}");
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
            var postzon = postzons.FirstOrDefault();
            if (postzon != null)
            {
                postalAddress.County = string.IsNullOrEmpty(postzon.CountyName) ? postzon.GovernmentOfficeName : postzon.CountyName;
                postalAddress.LocalAuthority = postzon.DistrictName;
                postalAddress.LocalAuthorityCodeName = postzon.DistrictCode;

                //TODO: Cache in service
                var localAuthority = _referenceDataService.GetLocalAuthorityByCode(postzon.DistrictCode);
                if (localAuthority != null)
                {
                    postalAddress.CountyId = localAuthority.County.CountyId;
                    postalAddress.County = localAuthority.County.FullName;
                    postalAddress.LocalAuthorityId = localAuthority.LocalAuthorityId;
                    postalAddress.LocalAuthorityCodeName = localAuthority.CodeName;
                    postalAddress.LocalAuthority = localAuthority.FullName;
                }
            }

            return postalAddress;
        }

        private static IEnumerable<string> GetAddressComponents(PostalAddress postalAddress)
        {
            var addressComponents = new List<string>(4) {postalAddress.AddressLine1};
            if (!string.IsNullOrEmpty(postalAddress.AddressLine2))
            {
                addressComponents.Add(postalAddress.AddressLine2);
            }
            if (!string.IsNullOrEmpty(postalAddress.AddressLine3))
            {
                addressComponents.Add(postalAddress.AddressLine3);
            }
            addressComponents.Add(postalAddress.Town);
            return addressComponents;
        }

        private static string GetAddressLine1(string primaryAddressableObject, string secondaryAddressableObject, string street)
        {
            if (!string.IsNullOrEmpty(secondaryAddressableObject))
            {
                return secondaryAddressableObject;
            }

            if (!string.IsNullOrEmpty(primaryAddressableObject))
            {
                return primaryAddressableObject;
            }

            return street;
        }

        private static string GetAddressLine2(string primaryAddressableObject, string secondaryAddressableObject, string street)
        {
            if (!string.IsNullOrEmpty(secondaryAddressableObject) && !string.IsNullOrEmpty(primaryAddressableObject))
            {
                return primaryAddressableObject;
            }

            if (string.IsNullOrEmpty(secondaryAddressableObject) && !string.IsNullOrEmpty(primaryAddressableObject))
            {
                return street;
            }

            return "";
        }

        private static string GetAddressLine3(string primaryAddressableObject, string secondaryAddressableObject, string street)
        {
            if (!string.IsNullOrEmpty(secondaryAddressableObject) && !string.IsNullOrEmpty(primaryAddressableObject))
            {
                return street;
            }

            return "";
        }

        private PcaFullAddress[] GetAddresses(IRestClient client, string resource)
        {
            //TODO: async
            //Retry up to three times with backoff
            var errorStringBuilder = new StringBuilder("Failed to retrieve address due to network error. The following errors occured:\r\n");
            for (var i = 0; i < 3; i++)
            {
                var request = new RestRequest(resource)
                {
                    RequestFormat = DataFormat.Json
                };

                var response = client.Execute(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    var addresses = JsonConvert.DeserializeObject<PcaFullAddress[]>(response.Content);
                    return addresses;
                }
                errorStringBuilder.AppendLine($"Request {i + 1}. ResponseStatus: {response.ResponseStatus} StatusCode {response.StatusCode}");
                Thread.Sleep(1000 * i);
            }

            var message = errorStringBuilder.ToString();
            _logger.Error(message);
            throw new CustomException(message, Postcode.ErrorCodes.PostalAddressRequestFailed);
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