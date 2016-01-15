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
    using RestSharp.Contrib;

    public class PostalAddressLookupProvider : RestService, IPostalAddressLookupProvider
    {
        private readonly ILogService _logger;
        private readonly IPostalAddressDetailsService _postalAddressDetailsService;
        private PostalAddressServiceConfiguration Config { get; }

        public PostalAddressLookupProvider(IConfigurationService configurationService, ILogService logger, IPostalAddressDetailsService postalAddressDetailsService)
        {
            _logger = logger;
            _postalAddressDetailsService = postalAddressDetailsService;
            Config = configurationService.Get<PostalAddressServiceConfiguration>();
            BaseUrl = new Uri(Config.FindByPartsEndpoint);
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
                    new KeyValuePair<string, string>("postcode", postcode)
                }
                );
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var addresses = Execute<List<PcaServiceFindResult>>(request);

            if (addresses.Data == null
                || !addresses.Data.Any()
                || addresses.Data.All(x => x.Id == null)) return null;

            var addressList = new List<PostalAddress>();

            foreach (var foundAddress in addresses.Data)
            {
                if (foundAddress.Id == null)
                    continue;

                var retrievedResult = _postalAddressDetailsService.RetrieveAddress(foundAddress.Id);
                if (retrievedResult != null)
                    addressList.Add(retrievedResult);
            }

            return addressList;
        }

        private static string GetFindByPartsServiceUrl()
        {
            return "&key={key}&Building={addressLine1}&Postcode={postcode}";
        }
    }
}
