namespace SFA.Apprenticeships.Infrastructure.EmployerDataService.Mappers
{
    using Domain.Entities.Locations;
    using EmployerDataService;

    public class BsAddressMapper
    {
        public Address ToAddress(BSaddressStructure fromAddress)
        {
            if (fromAddress == null)
            {
                return null;
            }

            return new Address
            {
                AddressLine1 = fromAddress.StreetDescription,
                AddressLine2 = fromAddress.PostTown,
                AddressLine3 = null,
                AddressLine4 = null,
                Postcode = fromAddress.PostCode,
                Uprn = fromAddress.UniquePropertyReferenceNumber,
                GeoPoint = null
            };
        }
    }
}
