namespace SFA.Apprenticeships.Web.Common.Providers
{
    using ViewModels.Locations;

    public interface IGoogleMapsProvider
    {
        string GetStaticMapsUrl(GeoPointViewModel geoPointViewModel);
    }
}