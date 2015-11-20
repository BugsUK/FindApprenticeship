namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Locations;
    using DataContracts.Version51;

    public interface IAddressMapper
    {
        AddressData MapToAddressData(Address address);
    }
}
