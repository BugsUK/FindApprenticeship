namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using Application.Location;
    using Configuration;
    using Domain.Entities.Raa.Locations;
    using Rest;
    using SFA.Infrastructure.Interfaces;

    public class GeoCodeLookupProvider : RestService, IGeoCodeLookupProvider
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogService _logService;
        private GeoCodingServiceConfiguration Config { get; }

        public GeoCodeLookupProvider(IConfigurationService configurationService, ILogService logService)
        {
            _configurationService = configurationService;
            _logService = logService;
            Config = configurationService.Get<GeoCodingServiceConfiguration>();
            BaseUrl = new Uri(Config.GeoCodingEndpoint);
        }

        public GeoPoint GetGeoCodingFor(PostalAddress address)
        {
            throw new System.NotImplementedException();
        }
    }
}