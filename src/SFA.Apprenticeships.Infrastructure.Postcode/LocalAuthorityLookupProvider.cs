namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using Application.Location;
    using Configuration;
    using SFA.Infrastructure.Interfaces;

    public class LocalAuthorityLookupProvider : ILocalAuthorityLookupProvider
    {
        private readonly ILogService _logService;
        private LocalAuthorityServiceConfiguration Config { get; }

        public LocalAuthorityLookupProvider(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;
            Config = configurationService.Get<LocalAuthorityServiceConfiguration>();
        }

        public string GetLocalAuthorityCode(string postcode)
        {
            _logService.Debug($"Calling LocalAuthorityLookupProvider to get local authority code for postcode: {postcode}");

            //TODO: OO: Once we obtain keys, implement service

            return "00CQ";

            ////Build the url
            //var url = Config.LocalAuthorityEndpoint;
            //url += "&Key=" + System.Web.HttpUtility.UrlEncode(Config.Key);
            //url += "&PostcodeOrPlace=" + System.Web.HttpUtility.UrlEncode(postcode);
            //url += "&UserName=" + System.Web.HttpUtility.UrlEncode(Config.Username);

            ////Create the dataset
            //var dataSet = new System.Data.DataSet();
            //dataSet.ReadXml(url);

            ////Check for an error
            //if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 &&
            //    dataSet.Tables[0].Columns[0].ColumnName == "Error")
            //    return null;

            ////Return the dataset
            //return dataSet.Tables[0].Rows[0].ItemArray[10].ToString();

            ////FYI: The dataset contains the following columns:
            ////Location
            ////Easting
            ////Northing
            ////Latitude
            ////Longitude
            ////OsGrid
            ////CountryCode
            ////CountryName
            ////CountyCode
            ////CountyName
            ////DistrictCode
            ////DistrictName
            ////WardCode
            ////WardName
            ////NhsShaCode
            ////NhsShaName
            ////NhsPctCode
            ////NhsPctName
            ////LeaCode
            ////LeaName
            ////GovernmentOfficeCode
            ////GovernmentOfficeName
            ////WestminsterConstituencyCode
            ////WestminsterConstituencyName
            ////WestminsterMP
            ////WestminsterParty
            ////WestminsterConstituencyCode2010
            ////WestminsterConstituencyName2010

        }
    }
}