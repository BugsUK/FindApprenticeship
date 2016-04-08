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
        //private readonly IConfigurationService _configurationService;
        //private readonly ILogService _logService;
        private GeoCodingServiceConfiguration Config { get; }

        public GeoCodeLookupProvider(IConfigurationService configurationService, ILogService logService)
        {
            //_configurationService = configurationService;
            //_logService = logService;
            //Config = configurationService.Get<GeoCodingServiceConfiguration>();
            //BaseUrl = new Uri(Config.GeoCodingEndpoint);
        }

        public GeoPoint GetGeoCodingFor(PostalAddress address)
        {
            // Clerkenwell Close, London -> returns values
            // 31 Clerkenwell Close, London -> doesn't return values
            // 31, Clerkenwell Close, London -> return values
            return new GeoPoint
            {
                Latitude = 53.4808,
                Longitude = -2.2426
            };
        }
    }
}