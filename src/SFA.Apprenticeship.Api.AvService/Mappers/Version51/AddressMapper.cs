namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Locations;
    using DataContracts.Version51;

    public static class AddressMapper
    {
        public static AddressData MapToAddressData(Address address)
        {
            return new AddressData
            {
                AddressLine1 = address.AddressLine1 ?? string.Empty
            };
        }
    }
}
