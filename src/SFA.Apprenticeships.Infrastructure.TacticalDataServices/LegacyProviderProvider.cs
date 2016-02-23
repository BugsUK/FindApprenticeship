using System.Diagnostics.Contracts;
using System.Text;
using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Application.Organisation;
    using Configuration;
    using Dapper;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using SFA.Infrastructure.Interfaces;

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
                ProviderId = legacyProvider.ProviderID,
                Ukprn = legacyProvider.UKPRN.ToString(),
                Name = legacyProvider.FullName
            };

            return provider;
        }

        public ProviderSite GetProviderSite(string ukprn, string edsUrn)
        {
            const string sql = @"SELECT ps.ProviderSiteID, p.UKPRN, ps.* 
                                 FROM dbo.Provider AS p 
                                 JOIN dbo.ProviderSiteRelationship AS psr ON p.ProviderID = psr.ProviderID 
                                 JOIN ProviderSite AS ps ON psr.ProviderSiteID = ps.ProviderSiteId 
                                 WHERE p.UKPRN = @Ukprn AND ps.EDSURN = @EdsUrn 
                                 AND ps.TrainingProviderStatusTypeId = 1";

            Models.ProviderSite legacyProviderSite;

            using (var connection = GetConnection())
            {
                legacyProviderSite = connection.Query<Models.ProviderSite>(sql, new { Ukprn = ukprn, EdsUrn = edsUrn }).SingleOrDefault();
            }

            return GetProviderSite(legacyProviderSite);
        }

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            const string sql = @"SELECT ps.ProviderSiteID, p.ProviderID, p.UKPRN, ps.* 
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

            var address = new PostalAddress
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
                ProviderSiteId = legacyProviderSite.ProviderSiteID,
                ProviderId = legacyProviderSite.ProviderID,
                Ukprn = legacyProviderSite.UKPRN.ToString(),
                EdsUrn = legacyProviderSite.EDSURN.ToString(),
                Name = legacyProviderSite.FullName,
                EmployerDescription = legacyProviderSite.EmployerDescription,
                CandidateDescription = legacyProviderSite.CandidateDescription,
                ContactDetailsForEmployer = legacyProviderSite.ContactDetailsForEmployer,
                ContactDetailsForCandidate = legacyProviderSite.ContactDetailsForCandidate,
                Address = address
            };

            return providerSite;
        }

        public VacancyParty GetVacancyParty(int providerSiteId, int employerId)
        {
            return null;
            /*var request = new EmployerSearchRequest(providerSiteId, employerId);
            var results = GetVacancyParties(request);
            return results.SingleOrDefault();*/
        }

        public VacancyParty GetVacancyParty(int providerSiteId, string edsUrn)
        {
            var request = new EmployerSearchRequest(providerSiteId, edsUrn);
            var results = GetProviderSiteEmployerLinks(request);
            return results.SingleOrDefault();
        }

        public IEnumerable<VacancyParty> GetProviderSiteEmployerLinks(int providerSiteId)
        {
            var request = new EmployerSearchRequest(providerSiteId);
            return GetProviderSiteEmployerLinks(request);
        }

        public IEnumerable<VacancyParty> GetProviderSiteEmployerLinks(EmployerSearchRequest searchRequest)
        {
            Contract.Requires(searchRequest != null);

            const string sql = @"SELECT * FROM dbo.VacancyOwnerRelationship AS vor WHERE vor.ProviderSiteID = @ProviderSiteId AND vor.StatusTypeId = 4";;

            IEnumerable<Models.VacancyOwnerRelationship> vacancyOwnerRelationships;

            using (var connection = GetConnection())
            {
                vacancyOwnerRelationships = connection.Query<Models.VacancyOwnerRelationship>(sql, new { searchRequest.ProviderSiteId });
            }

            return vacancyOwnerRelationships.Select(GetVacancyParty);
        }

        private static VacancyParty GetVacancyParty(Models.VacancyOwnerRelationship vacancyOwnerRelationship)
        {
            if (vacancyOwnerRelationship == null)
            {
                return null;
            }

            var providerSiteEmployerLink = new VacancyParty
            {
                VacancyPartyId = vacancyOwnerRelationship.VacancyOwnerRelationshipId,
                ProviderSiteId = vacancyOwnerRelationship.ProviderSiteId,
                EmployerId = vacancyOwnerRelationship.EmployerId,
                EmployerDescription = CleanDescription(vacancyOwnerRelationship.EmployerDescription),
                EmployerWebsiteUrl = vacancyOwnerRelationship.EmployerWebsite
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

        private IDbConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}