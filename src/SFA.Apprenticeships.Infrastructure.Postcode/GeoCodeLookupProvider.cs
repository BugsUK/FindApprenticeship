namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Data;
    using System.Linq;
    using Application.Location;
    using Configuration;
    using Domain.Entities.Raa.Locations;

    using Application.Interfaces;
    using Domain.Entities.Exceptions;

    public class GeoCodeLookupProvider : IGeoCodeLookupProvider
    {
        private readonly ILogService _logService;
        private GeoCodingServiceConfiguration Config { get; }
        private const string ErrorDataSetName = "ErrorDataSet";

        public GeoCodeLookupProvider(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;
            Config = configurationService.Get<GeoCodingServiceConfiguration>();
        }

        public GeoPoint GetGeoCodingFor(PostalAddress address)
        {
            _logService.Debug("Calling GeoCodeLookupProvider to geocode address {0}", address.ToString());

            var geoPoint = GetGeoPointFor(address.Postcode);

            if (!geoPoint.IsSet() && AddressIsSet(address))
            {
                geoPoint = GetGeoPointFor(GetServiceAddressFrom(address));
            }

            return geoPoint;
        }

        private bool AddressIsSet(PostalAddress address)
        {
            return address.AddressLine1 != null;
        }

        private string GetServiceAddressFrom(PostalAddress address)
        {
            var addressLine1Splitted = address.AddressLine1.Split(' ');
            int doorNumber;
            var startsWithNumber = int.TryParse(addressLine1Splitted.First(), out doorNumber);
            var numberOfParts = addressLine1Splitted.Length;

            var newAddressLine1 = startsWithNumber && numberOfParts > 1
                ? $"{doorNumber}, {string.Join(" ", addressLine1Splitted.Skip(1))}"
                : address.AddressLine1;

            var addressLines = new[]
                {newAddressLine1, address.AddressLine2, address.AddressLine3, address.AddressLine4, address.AddressLine5};

            return string.Join(",", addressLines.Where(line => !string.IsNullOrWhiteSpace(line)));
        }

        private GeoPoint GetGeoPointFor(string addressOrPostCode)
        {
            if(string.IsNullOrEmpty(addressOrPostCode)) return GeoPoint.NotSet;

            var dataSet = GetDataFromService(BuildUrl(addressOrPostCode));

            //Check for an error
            if (IsThereAnError(dataSet))
            {
                LogError(addressOrPostCode, dataSet);
                throw new CustomException(Application.Interfaces.Locations.ErrorCodes.GeoCodeLookupProviderFailed);
            }

            return NoDataPresent(dataSet) ? GeoPoint.NotSet : CreateGeoPointFrom(dataSet);
        }

        private static GeoPoint CreateGeoPointFrom(DataSet dataSet)
        {
            return new GeoPoint
            {
                Easting = int.Parse(dataSet.Tables[0].Rows[0].ItemArray[1].ToString()),
                Northing = int.Parse(dataSet.Tables[0].Rows[0].ItemArray[2].ToString()),
                Latitude = double.Parse(dataSet.Tables[0].Rows[0].ItemArray[3].ToString()),
                Longitude = double.Parse(dataSet.Tables[0].Rows[0].ItemArray[4].ToString()),
            };
        }

        private static bool NoDataPresent(DataSet dataSet)
        {
            return dataSet.Tables.Count == 0;
        }

        private DataSet GetDataFromService(string url)
        {
            try
            {
                var dataSet = new DataSet();
                dataSet.ReadXml(url);
                return dataSet;
            }
            catch (Exception ex)
            {
                _logService.Error($"An error has occcurred accessing GeoCode service on the address: {url}", ex);
                return new DataSet(ErrorDataSetName);
            }
        }

        private void LogError(string addressOrPostCode, DataSet dataSet)
        {
            _logService.Error("An error has occurred accessing GeoCode service for location {0}: {1}",
                addressOrPostCode, dataSet.Tables[0].Rows[0].ItemArray[1].ToString());
        }

        private string BuildUrl(string addressOrPostCode)
        {
            var url = Config.GeoCodingEndpoint;
            url += "&Key=" + System.Web.HttpUtility.UrlEncode(Config.Key);
            url += "&Location=" + System.Web.HttpUtility.UrlEncode(addressOrPostCode);

            return url;
        }

        private static bool IsThereAnError(DataSet dataSet)
        {
            return dataSet.DataSetName == ErrorDataSetName || (
                    dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 &&
                   dataSet.Tables[0].Columns[0].ColumnName == "Error");
        }
    }
}