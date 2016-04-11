namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;

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
            var employer = _employerService.GetEmployer(employerId);

            const bool testing = false;

            if (employer.Address.GeoPoint == null || testing)
            {
                var geoPoint = _geoCodeLookupService.GetGeoPointFor(employer.Address);

                return !geoPoint.IsSet() ? GeoCodeAddressResult.InvalidAddress : GeoCodeAddressResult.Ok;
            }

            return GeoCodeAddressResult.Ok;
        }
    }
}