namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public interface IGeoCodingProvider
    {
        GeoCodeAddressResult GeoCodeAddress(int employerId);
    }
}