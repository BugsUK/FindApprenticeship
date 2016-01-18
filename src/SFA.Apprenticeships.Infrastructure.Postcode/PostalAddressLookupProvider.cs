﻿namespace SFA.Apprenticeships.Infrastructure.Postcode
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
                    new KeyValuePair<string, string>("key", System.Web.HttpUtility.UrlEncode(Config.Key)),
                    new KeyValuePair<string, string>("addressLine1", addressLine1),
                    new KeyValuePair<string, string>("postcode", postcode)
                }
                );
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var addresses = Execute<List<PcaServiceFindResult>>(request);

            //In the case of an error, the Data property returned will be an empty list of length 1.
            //This is a failing of RestSharp, IMHO. Although the raw content is also returned,
            //verifyng the raw JSON is an unsavoury operation. The scenarios tested here should all
            //return null, so this is valid code, though the last conditionn may be a bit of an inference.
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
            return "&key={key}&Street={addressLine1}&Postcode={postcode}";
        }
    }
}
