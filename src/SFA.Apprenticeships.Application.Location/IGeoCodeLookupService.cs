namespace SFA.Apprenticeships.Application.Location
{
    using Domain.Entities.Raa.Locations;

    public interface IGeoCodeLookupService
    {
        GeoPoint GetGeoPointFor(PostalAddress address);
    }
}