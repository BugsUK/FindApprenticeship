namespace SFA.Apprenticeships.Application.Services.LocationSearchService.Extensions
{
    using System.Globalization;
    using Entities;
    using Domain.Entities;

    public static class DeliveryPointAddressExtensions
    {
        private static readonly TextInfo UkTextInfo = new CultureInfo("en-GB", false).TextInfo;

        public static Location ToLocation(this DeliveryPointAddress dpa)
        {
            var address = new Address
            {
                CompanyName = dpa.OrganisationName.ToTitleCase(),
                AddressLine2 = dpa.Village.ToTitleCase(),
                City = dpa.TownOrCity.ToTitleCase(),
                Uprn = dpa.Uprn,
                Postcode = dpa.Postcode
            };

            var buildingName = GetBuildingName(dpa);
            var streetAddress = GetStreetAddress(dpa);

            if (!string.IsNullOrEmpty(buildingName))
            {
                address.AddressLine1 = buildingName.ToTitleCase();
                if (!string.IsNullOrEmpty(streetAddress))
                {
                    address.AddressLine2 = streetAddress.ToTitleCase();
                    address.AddressLine3 = dpa.Village.ToTitleCase();
                }
            }
            else if (!string.IsNullOrEmpty(streetAddress))
            {
                address.AddressLine1 = streetAddress.ToTitleCase();
            }

            return new Location {Address = address};
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
            return dpa.ClassificationCode == OrdnanceSurveyClassificationCodes.ParentShell.PrimaryCode ||
                   dpa.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.Residential.PrimaryCode) ||
                   dpa.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.DualUse.PrimaryCode);
        }

        public static bool IsCommercial(this DeliveryPointAddress dpa)
        {
            return dpa.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.Commercial.PrimaryCode) ||
                   dpa.ClassificationCode.StartsWith(OrdnanceSurveyClassificationCodes.DualUse.PrimaryCode);
        }
    }
}