namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.Location;
    using Configuration;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Configuration;
    using Entities;
    using Rest;

    public class PostalAddressLookupProvider : RestService, IPostalAddressLookupProvider
    {
        private readonly ILogService _logger;
        private readonly IRetrieveAddressService _retrieveAddressService;
        private AddressConfiguration Config { get; }

        public PostalAddressLookupProvider(IConfigurationService configurationService, ILogService logger, IRetrieveAddressService retrieveAddressService)
        {
            _logger = logger;
            _retrieveAddressService = retrieveAddressService;
            Config = configurationService.Get<AddressConfiguration>();
            BaseUrl = new Uri(Config.FindByPartsServiceEndpoint);
        }

        public IEnumerable<PostalAddress> GetPostalAddresses(string addressLine1, string postcode)
        {
            Condition.Requires(addressLine1, "addressLine1").IsNotNullOrWhiteSpace();
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            _logger.Debug("Calling GetPostalAddresses for an address with addressLine1={0} and postcode={1}", addressLine1, postcode);

            var request = Create(GetFindByPartsServiceUrl()
                , new[]
                {
                    new KeyValuePair<string, string>("key", System.Web.HttpUtility.UrlEncode("JY37-NM56-JA37-WT99")),
                    new KeyValuePair<string, string>("addressLine1", addressLine1),
                    new KeyValuePair<string, string>("postcode", postcode),
                }
                );

            var addresses = Execute<List<FindPostalAddressByPartsResult>>(request);

            if (addresses.Data == null) return null;

            var addressList = new List<PostalAddress>();

            foreach (var foundAddress in addresses.Data)
            {
                var retrievedResult = _retrieveAddressService.RetrieveAddress(foundAddress.Id);
                if (retrievedResult != null)
                addressList.Add(new PostalAddress()
                {
                    AddressLine1 = retrievedResult.AddressLine1,
                    AddressLine2 = retrievedResult.AddressLine2,
                    AddressLine3 = retrievedResult.AddressLine3,
                    AddressLine4 = retrievedResult.AddressLine4,
                    ValidationSourceCode = "PCA",
                    ValidationSourceKeyValue = retrievedResult.Uprn,
                    Postcode = retrievedResult.Postcode,
                    GeoPoint = retrievedResult.GeoPoint,
                    DateValidated = DateTime.UtcNow
                });
            }

            return addressList;
        }

        private static string GetFindByPartsServiceUrl()
        {
            return "?key={key}&Building={addressLine1}&Postcode={postcode}";
        }
    }
}
