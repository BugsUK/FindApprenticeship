namespace SFA.Apprenticeships.Infrastructure.Address.Extensions
{
    using System.Globalization;
    using Domain.Entities.Locations;
    using Entities;

    public static class LocalPropertyIdentifierExtensions
    {
        private static readonly TextInfo UkTextInfo = new CultureInfo("en-GB", false).TextInfo;

        public static Address ToAddress(this LocalPropertyIdentifier lpi)
        {
            //https://github.com/alphagov/location-data-importer
            var address = new Address
            {
                AddressLine3 = lpi.TownOrCity.ToTitleCase(),
                AddressLine4 = lpi.TownOrCity != lpi.County ? lpi.County.ToTitleCase() : null,
                Postcode = lpi.Postcode,
                Uprn = lpi.Uprn,
                GeoPoint = new GeoPoint()
            };

            var companyName = lpi.OrganisationName;
            var buildingName = GetBuildingName(lpi);
            var streetAddress = GetStreetAddress(lpi);

            if (!string.IsNullOrEmpty(companyName))
            {
                address.AddressLine1 = companyName.ToTitleCase();
                if (!string.IsNullOrEmpty(buildingName) && companyName != buildingName)
                {
                    address.AddressLine1 += ", " + buildingName.ToTitleCase();
                    if (!string.IsNullOrEmpty(streetAddress))
                    {
                        address.AddressLine2 = streetAddress.ToTitleCase();
                    }
                }
                if (!string.IsNullOrEmpty(streetAddress))
                {
                    address.AddressLine2 = streetAddress.ToTitleCase();
                }
            }
            else if (!string.IsNullOrEmpty(buildingName))
            {
                address.AddressLine1 = buildingName.ToTitleCase();
                if (!string.IsNullOrEmpty(streetAddress))
                {
                    address.AddressLine2 = streetAddress.ToTitleCase();
                }
            }
            else if (!string.IsNullOrEmpty(streetAddress))
            {
                address.AddressLine1 = streetAddress.ToTitleCase();
            }
            
            return address;
        }

        private static string GetBuildingName(LocalPropertyIdentifier lpi)
        {
            if (lpi.SubBuildingNumber.HasValue || !string.IsNullOrEmpty(lpi.SubBuildingName))
            {
                var subBuildingDescription = lpi.SubBuildingNumber.HasValue ? lpi.SubBuildingNumber.Value.ToString() : lpi.SubBuildingName;

                if (string.IsNullOrEmpty(lpi.BuildingName)) return subBuildingDescription;

                return subBuildingDescription + ", " + lpi.BuildingName;
            }
            
            if (!string.IsNullOrEmpty(lpi.BuildingName))
            {
                return lpi.BuildingName;
            }
            
            return null;
        }

        private static string GetStreetAddress(LocalPropertyIdentifier lpi)
        {
            var streetAddress = lpi.BuildingNumber.HasValue ? lpi.BuildingNumber + lpi.BuildingNumberSuffix + " " + lpi.Street : lpi.Street;
            if (!string.IsNullOrEmpty(lpi.Village) && lpi.Village != lpi.County)
            {
                return streetAddress + ", " + lpi.Village;
            }
            return streetAddress;
        }

        private static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (value.Contains("&"))
            {
                value = value.Replace("&", "AND");
            }

            return UkTextInfo.ToTitleCase(UkTextInfo.ToLower(value));
        }

        public static bool IsResidential(this LocalPropertyIdentifier lpi)
        {
            return (lpi.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.ParentShell.PrimaryCode) ||
                   lpi.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.Residential.PrimaryCode) ||
                   lpi.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.DualUse.PrimaryCode)) &&
                   lpi.PostalAddressCode != "N";
        }
    }
}