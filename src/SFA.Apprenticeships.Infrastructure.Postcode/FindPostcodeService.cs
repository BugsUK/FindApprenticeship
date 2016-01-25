namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using CuttingEdge.Conditions;
    using Entities;
    using Rest;

    public class FindPostcodeService : RestService, IFindPostcodeService
    {
        private readonly ILogService _logger;
        private AddressConfiguration Config { get; }

        public FindPostcodeService(IConfigurationService configurationService, ILogService logger)
        {
            _logger = logger;
            Config = configurationService.Get<AddressConfiguration>();
            BaseUrl = new Uri(Config.FindServiceEndpoint);
        }

        public IEnumerable<PostcodeSearchInfo> FindPostcodes(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            _logger.Debug("Calling GetPossibleAddresses for Postcode={0}", postcode);

            var findRequest = Create(GetFindServiceUrl(),
                new[]
                {
                    new KeyValuePair<string, string>("Key", System.Web.HttpUtility.UrlEncode("JY37-NM56-JA37-WT99")),
                    new KeyValuePair<string, string>("SearchFor", System.Web.HttpUtility.UrlEncode("Everything")),
                    new KeyValuePair<string, string>("SearchTerm", System.Web.HttpUtility.UrlEncode(postcode)),
                    new KeyValuePair<string, string>("LastId",System.Web.HttpUtility.UrlEncode(string.Empty)),
                    new KeyValuePair<string, string>("Country", System.Web.HttpUtility.UrlEncode("GBR"))
                }
                );

            var postcodes = Execute<PostcodeSearchInfoResult>(findRequest);

            if (postcodes.Data != null && postcodes.Data.Items != null)
            {
                return postcodes.Data.Items.AsEnumerable();
            }

            return Enumerable.Empty<PostcodeSearchInfo>();
        }

        private static string GetFindServiceUrl()
        {
            return "&Key={Key}&SearchTerm={SearchTerm}&LastId={LastId}&SearchFor={SearchFor}&Country={Country}";
        }
    }
}