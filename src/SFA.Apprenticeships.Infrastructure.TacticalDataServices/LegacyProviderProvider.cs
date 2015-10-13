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
    using Domain.Entities.Providers;
    using Domain.Interfaces.Configuration;
    using Provider = Domain.Entities.Providers.Provider;
    using ProviderSite = Domain.Entities.Providers.ProviderSite;

    public class LegacyProviderProvider : ILegacyProviderProvider
    {
        private readonly string _connectionString;

        public LegacyProviderProvider(IConfigurationService configurationService)
        {
            var config = configurationService.Get<TacticalDataServivcesConfiguration>();
            _connectionString = config.AvSqlReferenceConnectionString;
        }

        public Provider GetProvider(string ukprn)
        {
            const string sql = @"SELECT * FROM dbo.Provider WHERE UKPRN = @Ukprn;";

            Models.Provider legacyProvider;

            using (var connection = GetConnection())
            {
                legacyProvider = connection.Query<Models.Provider>(sql, new {Ukprn = ukprn}).SingleOrDefault();
            }

            return GetProvider(legacyProvider);
        }

        private static Provider GetProvider(Models.Provider legacyProvider)
        {
            if (legacyProvider == null)
            {
                return null;
            }

            var provider = new Provider
            {
                Ukprn = legacyProvider.UKPRN.ToString(),
                Name = legacyProvider.FullName
            };

            return provider;
        }

        public ProviderSite GetProviderSite(string ukprn, string ern)
        {
            const string sql = @"SELECT p.UKPRN, ps.* 
                                 FROM dbo.Provider AS p 
                                 JOIN dbo.ProviderSiteRelationship AS psr ON p.ProviderID = psr.ProviderID 
                                 JOIN ProviderSite AS ps ON psr.ProviderSiteID = ps.ProviderSiteId 
                                 WHERE p.UKPRN = @Ukprn AND ps.EDSURN = @Ern 
                                 AND ps.TrainingProviderStatusTypeId = 1";

            Models.ProviderSite legacyProviderSite;

            using (var connection = GetConnection())
            {
                legacyProviderSite = connection.Query<Models.ProviderSite>(sql, new { Ukprn = ukprn, Ern = ern }).SingleOrDefault();
            }

            return GetProviderSite(legacyProviderSite);
        }

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            const string sql = @"SELECT p.UKPRN, ps.* 
                                 FROM dbo.Provider AS p 
                                 JOIN dbo.ProviderSiteRelationship AS psr ON p.ProviderID = psr.ProviderID 
                                 JOIN ProviderSite AS ps ON psr.ProviderSiteID = ps.ProviderSiteId 
                                 WHERE p.UKPRN = @Ukprn AND ps.TrainingProviderStatusTypeId = 1";

            IList<Models.ProviderSite> legacyProviderSites;

            using (var connection = GetConnection())
            {
                legacyProviderSites = connection.Query<Models.ProviderSite>(sql, new { Ukprn = ukprn }).ToList();
            }

            return legacyProviderSites.Select(GetProviderSite);
        }

        private static ProviderSite GetProviderSite(Models.ProviderSite legacyProviderSite)
        {
            if (legacyProviderSite == null)
            {
                return null;
            }

            var address = new Address
            {
                AddressLine1 = legacyProviderSite.AddressLine1,
                AddressLine2 = legacyProviderSite.AddressLine2,
                AddressLine3 = legacyProviderSite.AddressLine3,
                AddressLine4 = legacyProviderSite.Town,
                Postcode = legacyProviderSite.PostCode,
                GeoPoint = new GeoPoint
                {
                    Latitude = legacyProviderSite.Latitude,
                    Longitude = legacyProviderSite.Longitude
                },
                //Uprn = 
            };

            var providerSite = new ProviderSite
            {
                Ukprn = legacyProviderSite.UKPRN.ToString(),
                Ern = legacyProviderSite.EDSURN.ToString(),
                Name = legacyProviderSite.FullName,
                EmployerDescription = legacyProviderSite.EmployerDescription,
                CandidateDescription = legacyProviderSite.CandidateDescription,
                ContactDetailsForEmployer = legacyProviderSite.ContactDetailsForEmployer,
                ContactDetailsForCandidate = legacyProviderSite.ContactDetailsForCandidate,
                Address = address
            };

            return providerSite;
        }

        public ProviderSiteEmployerLink GetProviderSiteEmployerLink(string providerSiteErn, string ern)
        {
            const string sql = @"SELECT ps.EDSURN AS ProviderSiteEdsUrn, vor.ContractHolderIsEmployer, vor.ManagerIsEmployer, vor.StatusTypeId, 
                                 vor.Notes, vor.EmployerDescription, vor.EmployerWebsite, vor.NationWideAllowed, e.* 
                                 FROM dbo.ProviderSite AS ps 
                                 JOIN dbo.VacancyOwnerRelationship AS vor ON ps.ProviderSiteID = vor.ProviderSiteId 
                                 JOIN dbo.Employer AS e on vor.EmployerId = e.EmployerId 
                                 WHERE ps.EDSURN = @ProviderSiteErn 
                                 AND ps.TrainingProviderStatusTypeId = 1 
                                 AND e.EdsUrn = @Ern 
                                 AND e.EmployerStatusTypeId = 1";

            Models.VacancyOwnerRelationship vacancyOwnerRelationship;

            using (var connection = GetConnection())
            {
                vacancyOwnerRelationship =
                    connection.Query<Models.VacancyOwnerRelationship, Models.Employer, Models.VacancyOwnerRelationship>(
                        sql, (vor, employer) =>
                        {
                            vor.Employer = employer;
                            return vor;
                        }, new {ProviderSiteErn = providerSiteErn, Ern = ern},
                        splitOn: "NationWideAllowed,EmployerId").SingleOrDefault();
            }

            return GetProviderSiteEmployerLink(vacancyOwnerRelationship);
        }

        public IEnumerable<ProviderSiteEmployerLink> GetProviderSiteEmployerLinks(string providerSiteErn)
        {
            const string sql = @"SELECT ps.EDSURN AS ProviderSiteEdsUrn, vor.ContractHolderIsEmployer, vor.ManagerIsEmployer, vor.StatusTypeId, 
                                 vor.Notes, vor.EmployerDescription, vor.EmployerWebsite, vor.NationWideAllowed, e.* 
                                 FROM dbo.ProviderSite AS ps 
                                 JOIN dbo.VacancyOwnerRelationship AS vor ON ps.ProviderSiteID = vor.ProviderSiteId 
                                 JOIN dbo.Employer AS e on vor.EmployerId = e.EmployerId 
                                 WHERE ps.EDSURN = @ProviderSiteErn 
                                 AND ps.TrainingProviderStatusTypeId = 1 
                                 AND e.EmployerStatusTypeId = 1";

            IList<Models.VacancyOwnerRelationship> vacancyOwnerRelationships;

            using (var connection = GetConnection())
            {
                vacancyOwnerRelationships = 
                    connection.Query<Models.VacancyOwnerRelationship, Models.Employer, Models.VacancyOwnerRelationship>(
                        sql, (vor, employer) =>
                        {
                            vor.Employer = employer;
                            return vor;
                        }, new { ProviderSiteErn = providerSiteErn },
                        splitOn: "NationWideAllowed,EmployerId").ToList();
            }

            return vacancyOwnerRelationships.Select(GetProviderSiteEmployerLink);
        }

        private static ProviderSiteEmployerLink GetProviderSiteEmployerLink(Models.VacancyOwnerRelationship vacancyOwnerRelationship)
        {
            if (vacancyOwnerRelationship == null)
            {
                return null;
            }

            var address = new Address
            {
                AddressLine1 = vacancyOwnerRelationship.Employer.AddressLine1,
                AddressLine2 = vacancyOwnerRelationship.Employer.AddressLine2,
                AddressLine3 = vacancyOwnerRelationship.Employer.AddressLine3,
                AddressLine4 = vacancyOwnerRelationship.Employer.Town,
                Postcode = vacancyOwnerRelationship.Employer.PostCode,
                GeoPoint = new GeoPoint
                {
                    Latitude = vacancyOwnerRelationship.Employer.Latitude,
                    Longitude = vacancyOwnerRelationship.Employer.Longitude
                },
                //Uprn = 
            };

            var employer = new Employer
            {
                Ern = vacancyOwnerRelationship.Employer.EdsUrn.ToString(),
                Name = vacancyOwnerRelationship.Employer.FullName,
                Address = address
            };

            string websiteUrl;
            var isWebsiteUrlWellFormed = TryParseWebsiteUrl(vacancyOwnerRelationship.EmployerWebsite, out websiteUrl);

            var providerSiteEmployerLink = new ProviderSiteEmployerLink
            {
                ProviderSiteErn = vacancyOwnerRelationship.ProviderSiteEdsUrn.ToString(),
                Ern = vacancyOwnerRelationship.Employer.EdsUrn.ToString(),
                Description = CleanDescription(vacancyOwnerRelationship.EmployerDescription),
                WebsiteUrl = websiteUrl,
                IsWebsiteUrlWellFormed = isWebsiteUrlWellFormed,
                Employer = employer
            };

            return providerSiteEmployerLink;
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