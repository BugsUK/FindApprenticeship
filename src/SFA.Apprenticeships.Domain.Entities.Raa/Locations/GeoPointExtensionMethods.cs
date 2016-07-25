namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    using System;

    public static class GeoPointExtensionMethods
    {
        public static bool IsValid(this GeoPoint geoPoint)
        {
            const double tolerance = 0.001;

            if (geoPoint == null) return false;

            return (geoPoint.Easting == 0
                        && geoPoint.Northing == 0
                        && Math.Abs(geoPoint.Latitude) < tolerance
                        && Math.Abs(geoPoint.Longitude) < tolerance) == false;
        }
    }
}