namespace SFA.Apprenticeships.Application.Services.LocationSearchService.Extensions
{
    using System.Globalization;
    using Domain.Entities;
    using Entities;

    public static class LocalPropertyIdentifierExtensions
    {
        private static readonly TextInfo UkTextInfo = new CultureInfo("en-GB", false).TextInfo;

        public static Location ToLocation(this LocalPropertyIdentifier lpi)
        {
            //https://github.com/alphagov/location-data-importer
            var address = new Address
            {
                CompanyName = lpi.OrganisationName.ToTitleCase(),
                AddressLine3 = lpi.Village.ToTitleCase(),
                City = lpi.TownOrCity.ToTitleCase(),
                Uprn = lpi.Uprn,
                Postcode = lpi.Postcode
            };

            var companyName = lpi.OrganisationName;
            var buildingName = GetBuildingName(lpi);
            var streetAddress = GetStreetAddress(lpi);

            if (!string.IsNullOrEmpty(companyName))
            {
                if (!string.IsNullOrEmpty(buildingName) && companyName != buildingName)
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

            return new Location {Address = address};
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
            return streetAddress;
        }

        private static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return UkTextInfo.ToTitleCase(UkTextInfo.ToLower(value));
        }

        public static bool IsResidential(this LocalPropertyIdentifier lpi)
        {
            return (lpi.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.ParentShell.PrimaryCode) ||
                   lpi.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.Residential.PrimaryCode) ||
                   lpi.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.DualUse.PrimaryCode)) &&
                   lpi.PostalAddressCode != "N";
        }

        public static bool IsCommercial(this LocalPropertyIdentifier lpi)
        {
            return (lpi.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.Commercial.PrimaryCode) ||
                   lpi.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.DualUse.PrimaryCode)) &&
                   lpi.PostalAddressCode != "N";
        }
    }
}