#pragma warning disable 612
namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;
    using Domain.Entities.Raa.Locations;

    public class GeoCodingProvider : IGeoCodingProvider
    {
        private readonly IEmployerService _employerService;
        private readonly IGeoCodeLookupService _geoCodeLookupService;

        public GeoCodingProvider(IEmployerService employerService, IGeoCodeLookupService geoCodeLookupService)
        {
            _employerService = employerService;
            _geoCodeLookupService = geoCodeLookupService;
        }

        public GeoCodeAddressResult EmployerHasAValidAddress(int employerId)
        {
            var employer = _employerService.GetEmployer(employerId, true);

            if (NoGeoPoint(employer.Address) || InvalidGeopoint(employer.Address))
            {
                var geoPoint = _geoCodeLookupService.GetGeoPointFor(employer.Address);

                return !geoPoint.IsSet() ? GeoCodeAddressResult.InvalidAddress : GeoCodeAddressResult.Ok;
            }

            return GeoCodeAddressResult.Ok;
        }

        private bool NoGeoPoint(PostalAddress address)
        {
            return address.GeoPoint == null;
        }

        private bool InvalidGeopoint(PostalAddress address)
        {
            const double tolerance = 0.001;

            return Math.Abs(address.GeoPoint.Latitude) < 0.0001 && Math.Abs(address.GeoPoint.Longitude) < tolerance;
        }
    }
}