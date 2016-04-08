namespace SFA.Apprenticeships.Application.Location
{
    using Domain.Entities.Raa.Locations;

    public interface IGeoCodeLookupProvider
    {
        GeoPoint GetGeoCodingFor(PostalAddress address);
    }
}