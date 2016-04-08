namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;
    using Application.Location;

    public class GeoCodingProvider : IGeoCodingProvider
    {
        private readonly IEmployerService _employerService;
        private readonly IGeoCodeLookupService _geoCodeLookupService;

        public GeoCodingProvider(IEmployerService employerService, IGeoCodeLookupService geoCodeLookupService)
        {
            _employerService = employerService;
            _geoCodeLookupService = geoCodeLookupService;
        }

        public GeoCodeAddressResult GeoCodeAddress(int employerId)
        {
            var employer = _employerService.GetEmployer(employerId);
            var geoPoint = _geoCodeLookupService.GetGeoPointFor(employer.Address);

            if (!geoPoint.IsValid())
            {
                return GeoCodeAddressResult.InvalidAddress;
            }

            employer.Address.GeoPoint = geoPoint;

            _employerService.SaveEmployer(employer);

            return GeoCodeAddressResult.Ok;
        }
    }
}