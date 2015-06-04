namespace SFA.Apprenticeships.Infrastructure.Address.Extensions
{
    using System.Globalization;
    using Domain.Entities.Locations;
    using Entities;

    public static class DeliveryPointAddressExtensions
    {
        private static readonly TextInfo UkTextInfo = new CultureInfo("en-GB", false).TextInfo;

        public static Address ToAddress(this DeliveryPointAddress dpa)
        {
            //https://github.com/alphagov/location-data-importer
            var address = new Address
            {
                AddressLine3 = dpa.Village.ToTitleCase(),
                AddressLine4 = dpa.TownOrCity.ToTitleCase(),
                Postcode = dpa.Postcode,
                Uprn = dpa.Uprn,
                GeoPoint = new GeoPoint()
            };

            var companyName = dpa.OrganisationName;
            var buildingName = GetBuildingName(dpa);
            var streetAddress = GetStreetAddress(dpa);

            if (!string.IsNullOrEmpty(companyName))
            {
                address.AddressLine1 = companyName.ToTitleCase();
                if (!string.IsNullOrEmpty(buildingName))
                {
                    address.AddressLine2 = buildingName.ToTitleCase();
                    if (!string.IsNullOrEmpty(streetAddress))
                    {
                        address.AddressLine3 = streetAddress.ToTitleCase();
                    }
                }
                else if (!string.IsNullOrEmpty(streetAddress))
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

        private static string GetBuildingName(DeliveryPointAddress dpa)
        {
            if (!string.IsNullOrEmpty(dpa.SubBuildingName))
            {
                if (string.IsNullOrEmpty(dpa.BuildingName)) return dpa.SubBuildingName;

                return dpa.SubBuildingName + ", " + dpa.BuildingName;
            }
            
            if (!string.IsNullOrEmpty(dpa.BuildingName))
            {
                return dpa.BuildingName;
            }
            
            return null;
        }

        private static string GetStreetAddress(DeliveryPointAddress dpa)
        {
            return dpa.BuildingNumber.HasValue ? dpa.BuildingNumber + " " + dpa.Street : dpa.Street;
        }

        private static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return UkTextInfo.ToTitleCase(UkTextInfo.ToLower(value));
        }

        public static bool IsResidential(this DeliveryPointAddress dpa)
        {
            return dpa.classificationCode.StartsWith(OrdnanceSurveyClassificationCodes.ParentShell.PrimaryCode) ||
                   dpa.classificationCode.StartsWith(OrdnanceSurveyClassificationCodes.Residential.PrimaryCode) ||
                   dpa.classificationCode.StartsWith(OrdnanceSurveyClassificationCodes.DualUse.PrimaryCode);
        }
    }
}