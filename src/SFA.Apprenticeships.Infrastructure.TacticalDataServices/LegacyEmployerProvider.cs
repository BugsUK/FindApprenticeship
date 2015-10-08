namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Application.Organisation;
    using Configuration;
    using Dapper;
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

        public IEnumerable<Employer> GetEmployers(string ern)
        {
            const string sql = @"SELECT ps.EDSURN AS ProviderSiteEdsUrn, vor.ContractHolderIsEmployer, vor.ManagerIsEmployer, vor.StatusTypeId, vor.Notes, vor.EmployerDescription, vor.EmployerWebsite, vor.NationWideAllowed, e.* FROM dbo.ProviderSite AS ps JOIN dbo.VacancyOwnerRelationship AS vor ON ps.ProviderSiteID = vor.ProviderSiteId JOIN dbo.Employer AS e on vor.EmployerId = e.EmployerId WHERE ps.EDSURN = @Ern AND ps.TrainingProviderStatusTypeId = 1 AND e.EmployerStatusTypeId = 1";

            IList<Models.Employer> legacyEmployers;

            using (var connection = GetConnection())
            {
                legacyEmployers = connection.Query<Models.Employer>(sql, new { Ern = ern }).ToList();
            }

            return legacyEmployers.Select(GetEmployer);
        }

        private static Employer GetEmployer(Models.Employer legacyEmployer)
        {
            if (legacyEmployer == null)
            {
                return null;
            }

            var employer = new Employer
            {
                ProviderSiteErn = legacyEmployer.ProviderSiteEdsUrn.ToString(),
                Ern = legacyEmployer.EdsUrn.ToString(),
                Name = legacyEmployer.FullName,
                Description = legacyEmployer.EmployerDescription,
                Website = legacyEmployer.EmployerWebsite
            };

            return employer;
        }

        private IDbConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}