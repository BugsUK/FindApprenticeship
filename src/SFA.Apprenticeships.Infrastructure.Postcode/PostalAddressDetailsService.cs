namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Locations;
    using Entities;
    using Rest;
    using SFA.Infrastructure.Interfaces;

    public class PostalAddressDetailsService : RestService, IPostalAddressDetailsService
    {
        private readonly ILogService _logger;
        private PostalAddressServiceConfiguration Config { get; }

        public PostalAddressDetailsService(IConfigurationService configurationService, ILogService logger)
        {
            _logger = logger;
            Config = configurationService.Get<PostalAddressServiceConfiguration>();
            BaseUrl = new Uri(Config.RetrieveByIdEndpoint);
        }

        public PostalAddress RetrieveValidatedAddress(string addressId)
        {
            Condition.Requires(addressId, "addressId").IsNotNullOrWhiteSpace();

            _logger.Debug("Calling RetrieveAddressById for an address with Id={0}", addressId);

            var request = Create(GetRetrieveServiceUrl()
                , new[]
                {
                    new KeyValuePair<string, string>("key", System.Web.HttpUtility.UrlEncode(Config.Key)),
                    new KeyValuePair<string, string>("id", addressId)
                }
                );
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var addresses = Execute<List<PcaAddress>>(request);

            if (addresses.Data == null
                || !addresses.Data.Any()
                || addresses.Data.Single().Udprn == null) return null;

            var address = addresses.Data.Single();

            var result = new PostalAddress
            {
                AddressLine1 = address.Line1,
                AddressLine2 = address.Line2,
                AddressLine3 = address.Line3,
                AddressLine4 = address.Line4,
                AddressLine5 = address.Line5,
                Postcode = address.Postcode,
                Town = address.PostTown,
                ValidationSourceKeyValue = address.Udprn,
                ValidationSourceCode = "PCA"
            };

            return result;
        }

        private static string GetRetrieveServiceUrl()
        {
            return "&key={key}&id={id}";
        }
    }
}
