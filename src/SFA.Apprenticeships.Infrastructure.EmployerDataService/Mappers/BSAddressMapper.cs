namespace SFA.Apprenticeships.Infrastructure.EmployerDataService.Mappers
{
    using System;
    using System.Linq;
    using Domain.Entities.Locations;
    using EmployerDataService;

    public class BsAddressMapper
    {
        public Address ToAddress(BSaddressStructure fromAddress)
        {
            if (fromAddress == null)
            {
                throw new ArgumentNullException(nameof(fromAddress));
            }

            // Reference: http://xml.coverpages.org/BS7666SchemaDefinition-20020312-xsd.txt.
            //  PAON: Primary Addressable Object Name
            //  SAON: Secondary Addressable Object Name
            var paon = fromAddress.PAON?.Items.FirstOrDefault()?.ToString();

            return new Address
            {
                AddressLine1 = MapAddressComponent(paon),
                AddressLine2 = MapAddressComponent(fromAddress.StreetDescription),
                AddressLine3 = MapAddressComponent(fromAddress.PostTown),
                AddressLine4 = null,
                Postcode = MapAddressComponent(fromAddress.PostCode),
                Uprn = null,
                GeoPoint = null
            };
        }

        #region Helpers

        private static string MapAddressComponent(string addressComponent)
        {
            return string.IsNullOrWhiteSpace(addressComponent) ? null : addressComponent.Trim();
        }

        #endregion
    }
}
