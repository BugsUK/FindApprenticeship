namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System.Data;
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

            var dataSet = GetDataFromService(BuildUrl(postcode));

            if (IsThereAnyError(dataSet))
                return null;

            return NoDataPresent(dataSet) ? null : dataSet.Tables[0].Rows[0].ItemArray[10].ToString();
        }

        private static bool NoDataPresent(DataSet dataSet)
        {
            return dataSet.Tables.Count == 0;
        }

        private static bool IsThereAnyError(DataSet dataSet)
        {
            return dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 &&
                   dataSet.Tables[0].Columns[0].ColumnName == "Error";
        }

        private static DataSet GetDataFromService(string url)
        {
            var dataSet = new System.Data.DataSet();
            dataSet.ReadXml(url);
            return dataSet;
        }

        private string BuildUrl(string postcode)
        {
            var url = Config.LocalAuthorityEndpoint;
            url += "&Key=" + System.Web.HttpUtility.UrlEncode(Config.Key);
            url += "&PostcodeOrPlace=" + System.Web.HttpUtility.UrlEncode(postcode);
            return url;
        }
    }
}