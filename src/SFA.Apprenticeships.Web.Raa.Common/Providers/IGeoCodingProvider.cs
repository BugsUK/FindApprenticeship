namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public interface IGeoCodingProvider
    {
        GeoCodeAddressResult EmployerHasAValidAddress(int employerId);
    }
}