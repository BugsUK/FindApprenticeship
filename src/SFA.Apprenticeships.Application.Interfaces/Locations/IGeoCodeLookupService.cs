namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System;
    using Domain.Entities.Raa.Locations;

    [Obsolete]
    public interface IGeoCodeLookupService
    {
        GeoPoint GetGeoPointFor(PostalAddress address);
    }
}