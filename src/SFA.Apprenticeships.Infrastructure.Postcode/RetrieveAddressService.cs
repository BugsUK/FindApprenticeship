namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Entities;
    using Rest;

    public class RetrieveAddressService : RestService, IRetrieveAddressService
    {
        private readonly ILogService _logger;
        private AddressConfiguration Config { get; }

        private Dictionary<string, string> _specialCities = new Dictionary<string, string>
        {
            { "London", "London"},
            { "York", "North Yorkshire"}
        };

        public RetrieveAddressService(IConfigurationService configurationService, ILogService logger)
        {
            _logger = logger;
            Config = configurationService.Get<AddressConfiguration>();
            BaseUrl = new Uri(Config.RetrieveServiceEndpoint);
        }

        public Address RetrieveAddress(string addressId)
        {
            Condition.Requires(addressId, "addressId").IsNotNullOrWhiteSpace();

            _logger.Debug("Calling RetrieveValidatedAddress for an address with Id={0}", addressId);

            var request = Create(GetRetrieveServiceUrl()
                , new[]
                {
                    new KeyValuePair<string, string>("key", System.Web.HttpUtility.UrlEncode("JY37-NM56-JA37-WT99")),
                    new KeyValuePair<string, string>("id", addressId)
                }
                );

            var addresses = Execute<AddressInfoResult>(request);

            if (addresses.Data?.Items == null) return null;

            var address = addresses.Data.Items.First();

            return new Address
            {
                AddressLine1 = address.Line1,
                AddressLine2 = address.Line2,
                AddressLine3 = address.Line3,
                AddressLine4 = address.Line4,
                Town = address.City,
                County = GetCounty(address),
                Postcode = address.PostalCode,
                Uprn = address.DomesticId
            };
        }

        private string GetCounty(AddressInfo address)
        {
            if (!string.IsNullOrWhiteSpace(address.ProvinceName)) return address.ProvinceName;

            if (_specialCities.ContainsKey(address.City))
            {
                return _specialCities[address.City];
            }

            return address.AdminAreaName;
        }
        
        private static string GetRetrieveServiceUrl()
        {
            return "&key={key}&id={id}";
        }
    }
}