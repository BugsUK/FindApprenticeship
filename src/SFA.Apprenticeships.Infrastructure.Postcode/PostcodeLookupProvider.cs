namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Application.Location;
    using Configuration;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Entities;
    using Rest;

    using SFA.Apprenticeships.Application.Interfaces;

    public class PostcodeLookupProvider : RestService, IPostcodeLookupProvider
    {
        private readonly ILogService _logger;

        public PostcodeLookupProvider(IConfigurationService configurationService, ILogService logger)
        {
            var config = configurationService.Get<PostcodeConfiguration>();
            BaseUrl = new Uri(config.ServiceEndpoint);
            _logger = logger;
        }

        public Location GetLocation(string postcode)
        {
            _logger.Debug("Calling GetLocation for Postcode={0}", postcode);
            var result = GetPartialMatches(postcode).FirstOrDefault();

            return result == null
                ? null
                : new Location
                {
                    Name = postcode,
                    GeoPoint = new GeoPoint {Latitude = result.Latitude, Longitude = result.Longitude}
                };
        }

        private IEnumerable<PostcodeInfo> GetPartialMatches(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            var request = Create(GetServiceUrl(postcode),
                new[] {new KeyValuePair<string, string>("postcode", postcode)});
            var postcodes = Execute<PostcodeInfoResult>(request);

            if (postcodes.Data != null && postcodes.Data.Result != null)
                return postcodes.Data.Result.AsEnumerable();

            return Enumerable.Empty<PostcodeInfo>();
        }

        private static string GetServiceUrl(string postcode)
        {
            if (LocationHelper.IsPostcode(postcode))
                return "postcodes?q={postcode}";

            return string.Format("outcodes/{0}", postcode);
        }
    }
}
