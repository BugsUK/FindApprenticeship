namespace SFA.Apprenticeships.Application.Location
{
    using Domain.Entities.Raa.Locations;
    using Interfaces.Locations;

    public class GeoCodeLookupService : IGeoCodeLookupService
    {
        private readonly IGeoCodeLookupProvider _geoCodeLookupProvider;

        public GeoCodeLookupService(IGeoCodeLookupProvider geoCodeLookupProvider)
        {
            _geoCodeLookupProvider = geoCodeLookupProvider;
        }

        public GeoPoint GetGeoPointFor(PostalAddress address)
        {
            return _geoCodeLookupProvider.GetGeoCodingFor(address);
        }
    }
}