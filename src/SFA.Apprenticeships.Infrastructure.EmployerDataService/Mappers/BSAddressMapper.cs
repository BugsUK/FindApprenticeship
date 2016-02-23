namespace SFA.Apprenticeships.Infrastructure.EmployerDataService.Mappers
{
    using System;
    using System.Linq;
    using Domain.Entities.Raa.Locations;
    using EmployerDataService;

    public class BsAddressMapper
    {
        public PostalAddress ToAddress(BSaddressStructure fromAddress)
        {
            if (fromAddress == null)
            {
                throw new ArgumentNullException(nameof(fromAddress));
            }

            // Reference: http://xml.coverpages.org/BS7666SchemaDefinition-20020312-xsd.txt.
            //  PAON: Primary Addressable Object Name
            //  SAON: Secondary Addressable Object Name
            var paon = fromAddress.PAON?.Items.FirstOrDefault()?.ToString();

            return new PostalAddress
            {
                AddressLine1 = MapAddressComponent(paon),
                AddressLine2 = MapAddressComponent(fromAddress.StreetDescription),
                AddressLine3 = MapAddressComponent(fromAddress.PostTown),
                AddressLine4 = null,
                Postcode = MapAddressComponent(fromAddress.PostCode),
                ValidationSourceCode = null,
                ValidationSourceKeyValue = null,
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
