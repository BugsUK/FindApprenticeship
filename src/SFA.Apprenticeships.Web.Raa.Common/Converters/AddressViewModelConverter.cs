namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Web.Common.ViewModels.Locations;
    using Domain.Entities.Locations;

    public static class AddressViewModelConverter
    {
        public static AddressViewModel Convert(this Address address)
        {
            var vieModel = new AddressViewModel
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                AddressLine4 = address.AddressLine4,
                Postcode = address.Postcode,
                GeoPoint = address.GeoPoint == null ? new GeoPointViewModel() : new GeoPointViewModel
                {
                    Latitude = address.GeoPoint.Latitude,
                    Longitude = address.GeoPoint.Longitude
                },
                Uprn = address.Uprn
            };

            return vieModel;
        }
    }
}