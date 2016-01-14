namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Logging;
    using Application.Location;
    using Configuration;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Configuration;
    using Rest;

    public class PostalAddressLookupProvider : RestService, IPostalAddressLookupProvider
    {
        private readonly ILogService _logger;
        private readonly RetrieveAddressService _retrieveAddressService;
        private AddressConfiguration Config { get; }

        public PostalAddressLookupProvider(IConfigurationService configurationService, ILogService logger, RetrieveAddressService retrieveAddressService)
        {
            _logger = logger;
            _retrieveAddressService = retrieveAddressService;
            Config = configurationService.Get<AddressConfiguration>();
            BaseUrl = new Uri(Config.RetrieveServiceEndpoint);
        }

        public IEnumerable<PostalAddress> GetPostalAddresses(string addressLine1, string postcode)
        {
            throw new NotImplementedException();
        }
    }
}
