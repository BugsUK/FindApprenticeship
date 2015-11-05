namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Application.ReferenceData;
    using Configuration;
    using Dapper;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Configuration;
    using Models;

    public class FrameworkDataProvider : IReferenceDataProvider
    {
        private readonly string _connectionString;

        public FrameworkDataProvider(IConfigurationService configurationService)
        {
            var config = configurationService.Get<TacticalDataServivcesConfiguration>();
            _connectionString = config.AvSqlReferenceConnectionString;
        }

        public IEnumerable<Category> GetCategories()
        {
            var sql = @"SELECT * FROM dbo.ApprenticeshipOccupation WHERE ApprenticeshipOccupationStatusTypeId = 1;
                        SELECT * FROM dbo.ApprenticeshipFramework WHERE ApprenticeshipFrameworkStatusTypeId = 1;";

            List<Framework> frameworks;
            List<Occupation> occupations;

            using (var connection = GetConnection())
            {
                using (var multi = connection.QueryMultiple(sql))
                {
                    occupations = multi.Read<Occupation>().ToList();
                    frameworks = multi.Read<Framework>().ToList();
                }
            }

            occupations.ForEach(o =>
            {
                o.Frameworks = frameworks.Where(f => f.ApprenticeshipOccupationId == o.ApprenticeshipOccupationId).ToList();
                o.Frameworks.ForEach(f => f.ParentCategoryCodeName = o.CodeName);
            });

            return occupations.Select(o => o.ToCategory()).ToList();
        }

        public Category GetSubCategoryByName(string subCategoryName)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.FullName == subCategoryName);
        }

        public Category GetCategoryByName(string categoryName)
        {
            return GetCategories().FirstOrDefault(c => c.FullName == categoryName);
        }

        public Category GetSubCategoryByCode(string subCategoryCode)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.CodeName == subCategoryCode);
        }

        public Category GetCategoryByCode(string categoryCode)
        {
            return GetCategories().FirstOrDefault(c => c.CodeName == categoryCode);
        }

        public IEnumerable<Sector> GetSectors()
        {
            const string sql = @"SELECT * FROM [dbo].[ApprenticeshipSector];
                                 SELECT * FROM [dbo].[ApprenticeshipStandard];";

            List<Sector> sectors;
            List<Standard> standards;

            using (var connection = GetConnection())
            {
                using (var multi = connection.QueryMultiple(sql))
                {
                    sectors = multi.Read<Sector>().ToList();
                    standards = multi.Read<Standard>().ToList();
                }
            }

            foreach (var sector in sectors)
            {
                sector.Standards = standards.Where(s => s.ApprenticeshipSectorId == sector.Id).ToList();
            }

            return sectors;
        }

        private IDbConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}