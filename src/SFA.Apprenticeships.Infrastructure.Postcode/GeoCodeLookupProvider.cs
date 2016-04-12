namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Linq;
    using Application.Location;
    using Configuration;
    using Domain.Entities.Raa.Locations;
    using Rest;
    using SFA.Infrastructure.Interfaces;

    public class GeoCodeLookupProvider : IGeoCodeLookupProvider
    {
        private readonly ILogService _logService;
        private GeoCodingServiceConfiguration Config { get; }

        public GeoCodeLookupProvider(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;
            Config = configurationService.Get<GeoCodingServiceConfiguration>();
        }

        public GeoPoint GetGeoCodingFor(PostalAddress address)
        {
            _logService.Debug("Calling GeoCodeLookupProvider to geocode address {0}", address.ToString());

            var geoPoint = GetGeoPointFor(address.Postcode);

            if (!geoPoint.IsSet())
            {
                geoPoint = GetGeoPointFor(GetServiceAddressFrom(address));
            }

            return geoPoint;
        }

        private string GetServiceAddressFrom(PostalAddress address)
        {
            // Clerkenwell Close, London -> returns values
            // 31 Clerkenwell Close, London -> doesn't return values
            // 31, Clerkenwell Close, London -> return values

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
            //TODO: OO: Once we have keys, implement this
            return new GeoPoint() {Longitude = -1.507874, Latitude = 52.401182, Easting = 433580, Northing = 278244 };

            // Code to test in an integration test

            ////Build the url
            //var url = Config.GeoCodingEndpoint;
            //url += "&Key=" + System.Web.HttpUtility.UrlEncode(Config.Key);
            //url += "&Location=" + System.Web.HttpUtility.UrlEncode(addressOrPostCode);

            ////Create the dataset
            //var dataSet = new System.Data.DataSet();
            //dataSet.ReadXml(url);

            ////Check for an error
            //if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 &&
            //    dataSet.Tables[0].Columns[0].ColumnName == "Error")
            //{
            //    _logService.Error("An error has occurred accessing GeoCode service for location {0}: {1}",
            //        addressOrPostCode, dataSet.Tables[0].Rows[0].ItemArray[1].ToString());

            //    return GeoPoint.NotSet;
            //}

            ////Return the dataset
            //// return dataSet;

            ////FYI: The dataset contains the following columns:
            ////Location
            ////Easting
            ////Northing
            ////Latitude
            ////Longitude
            ////OsGrid
            ////Accuracy

            //return new GeoPoint
            //{
            //    Easting = int.Parse(dataSet.Tables[0].Rows[0].ItemArray[1].ToString()),
            //    Northing = int.Parse(dataSet.Tables[0].Rows[0].ItemArray[2].ToString()),
            //    Latitude = double.Parse(dataSet.Tables[0].Rows[0].ItemArray[3].ToString()),
            //    Longitude = double.Parse(dataSet.Tables[0].Rows[0].ItemArray[4].ToString()),
            //};

        }
    }
}