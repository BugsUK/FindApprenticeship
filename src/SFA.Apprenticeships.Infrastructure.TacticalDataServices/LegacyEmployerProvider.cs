namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Application.Organisation;
    using Configuration;
    using Dapper;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Interfaces.Configuration;

    public class LegacyEmployerProvider : ILegacyEmployerProvider
    {
        private readonly string _connectionString;

        public LegacyEmployerProvider(IConfigurationService configurationService)
        {
            var config = configurationService.Get<TacticalDataServivcesConfiguration>();
            _connectionString = config.AvSqlReferenceConnectionString;
        }

        public Employer GetEmployer(string providerSiteErn, string ern)
        {
            const string sql = @"SELECT ps.EDSURN AS ProviderSiteEdsUrn, vor.ContractHolderIsEmployer, vor.ManagerIsEmployer, vor.StatusTypeId, vor.Notes, vor.EmployerDescription, vor.EmployerWebsite, vor.NationWideAllowed, e.* FROM dbo.ProviderSite AS ps JOIN dbo.VacancyOwnerRelationship AS vor ON ps.ProviderSiteID = vor.ProviderSiteId JOIN dbo.Employer AS e on vor.EmployerId = e.EmployerId WHERE ps.EDSURN = @ProviderSiteErn AND ps.TrainingProviderStatusTypeId = 1 AND e.EdsUrn = @Ern AND e.EmployerStatusTypeId = 1";

            Models.Employer legacyEmployer;

            using (var connection = GetConnection())
            {
                legacyEmployer = connection.Query<Models.Employer>(sql, new { ProviderSiteErn = providerSiteErn, Ern = ern }).SingleOrDefault();
            }

            return GetEmployer(legacyEmployer);
        }

        public IEnumerable<Employer> GetEmployers(string providerSiteErn)
        {
            const string sql = @"SELECT ps.EDSURN AS ProviderSiteEdsUrn, vor.ContractHolderIsEmployer, vor.ManagerIsEmployer, vor.StatusTypeId, vor.Notes, vor.EmployerDescription, vor.EmployerWebsite, vor.NationWideAllowed, e.* FROM dbo.ProviderSite AS ps JOIN dbo.VacancyOwnerRelationship AS vor ON ps.ProviderSiteID = vor.ProviderSiteId JOIN dbo.Employer AS e on vor.EmployerId = e.EmployerId WHERE ps.EDSURN = @ProviderSiteErn AND ps.TrainingProviderStatusTypeId = 1 AND e.EmployerStatusTypeId = 1";

            IList<Models.Employer> legacyEmployers;

            using (var connection = GetConnection())
            {
                legacyEmployers = connection.Query<Models.Employer>(sql, new { ProviderSiteErn = providerSiteErn }).ToList();
            }

            return legacyEmployers.Select(GetEmployer);
        }

        private static Employer GetEmployer(Models.Employer legacyEmployer)
        {
            if (legacyEmployer == null)
            {
                return null;
            }

            var address = new Address
            {
                AddressLine1 = legacyEmployer.AddressLine1,
                AddressLine2 = legacyEmployer.AddressLine2,
                AddressLine3 = legacyEmployer.AddressLine3,
                AddressLine4 = legacyEmployer.Town,
                Postcode = legacyEmployer.PostCode,
                GeoPoint = new GeoPoint
                {
                    Latitude = legacyEmployer.Latitude,
                    Longitude = legacyEmployer.Longitude
                },
                //Uprn = 
            };

            string websiteUrl;
            var isWebsiteUrlWellFormed = TryParseWebsiteUrl(legacyEmployer.EmployerWebsite, out websiteUrl);

            var employer = new Employer
            {
                ProviderSiteErn = legacyEmployer.ProviderSiteEdsUrn.ToString(),
                Ern = legacyEmployer.EdsUrn.ToString(),
                Name = legacyEmployer.FullName,
                Description = CleanDescription(legacyEmployer.EmployerDescription),
                WebsiteUrl = websiteUrl,
                IsWebsiteUrlWellFormed = isWebsiteUrlWellFormed,
                Address = address
            };

            return employer;
        }

        private static string CleanDescription(string description)
        {
            if (description == null)
            {
                return "";
            }

            description = Regex.Replace(description, @"^\s*", "");
            description = Regex.Replace(description, @"\s*$", "");
            description = Regex.Replace(description, @"<br.*?>", "\r\n");
            description = Regex.Replace(description, @"&nbsp;", " ");
            description = Regex.Replace(description, @"&amp;", "&");
            description = Regex.Replace(description, @"&pound;", "£");
            description = Regex.Replace(description, @"<[^>]+>|&nbsp;", "");
            description = Regex.Replace(description, @"\s{2,}", " ");

            return description;
        }

        private static bool TryParseWebsiteUrl(string website, out string websiteUrl)
        {
            websiteUrl = website;
            if (IsValidUrl(website))
            {
                websiteUrl = new UriBuilder(website).Uri.ToString();
                return true;
            }
            return false;
        }

        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            try
            {
                // Attempting to build the URL will throw an exception if it is invalid.
                // ReSharper disable once UnusedVariable
                var unused = new UriBuilder(url);

                return true;
            }
            catch (UriFormatException)
            {
                return false;
            }
        }

        private IDbConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}