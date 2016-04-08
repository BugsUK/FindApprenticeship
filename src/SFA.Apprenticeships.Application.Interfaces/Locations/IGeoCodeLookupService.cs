namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using Domain.Entities.Raa.Locations;

    public interface IGeoCodeLookupService
    {
        GeoPoint GetGeoPointFor(PostalAddress address);
    }
}