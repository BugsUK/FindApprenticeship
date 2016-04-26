namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System.Collections.Generic;
    using Infrastructure.Repositories.Sql.Common;

    public class VacancyRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public VacancyRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public HashSet<int> GetAllVacancyIds()
        {
            return new HashSet<int>(_getOpenConnection.Query<int>("SELECT VacancyId FROM Vacancy"));
        }

        public IDictionary<string, int> GetAllVacancyLocalAuthorities()
        {
            var vacancyLocalAuthorities = new Dictionary<string, int>(100000);

            for(var i = 1; i <= 3; i++)
            {
                var sql = GetAllVacancyLocalAuthoritiesSql(i);
                foreach (var vacancyLocalAuthority in _getOpenConnection.Query<VacancyLocalAuthority>(sql))
                {
                    vacancyLocalAuthorities[vacancyLocalAuthority.PostCodePrefix] = vacancyLocalAuthority.LocalAuthorityId;
                }
            }

            return vacancyLocalAuthorities;
        }

        private static string GetAllVacancyLocalAuthoritiesSql(int rightTrimChars)
        {
            return $@"
SELECT REPLACE(SUBSTRING(PostCode, 1, LEN(PostCode) - {rightTrimChars}), ' ', '') as PostCodePrefix, LocalAuthorityId, COUNT(LocalAuthorityId) as LocalAuthorityIdCount 
FROM Vacancy WHERE LocalAuthorityId <> 0 
AND PostCode IS NOT NULL 
AND PostCode <> '' 
GROUP BY REPLACE(SUBSTRING(PostCode, 1, LEN(PostCode) - {rightTrimChars}), ' ', ''), LocalAuthorityId 
ORDER BY PostCodePrefix, LocalAuthorityIdCount";
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class VacancyLocalAuthority
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public string PostCodePrefix { get; set; }
            public int LocalAuthorityId { get; set; }
            public int LocalAuthorityIdCount { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}