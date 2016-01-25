namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Domain.Entities.Locations;

    public static class AddressPresenter
    {
        public static string GetCityOrTownDisplayText(this Address address)
        {
            return address.AddressLine4 ?? address.AddressLine3 ?? address.AddressLine2 ?? address.AddressLine1;
        }
    }
}