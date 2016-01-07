namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper;

    using System.Data.SqlClient;
    using System.Data;
    using Vacancy = SFA.Apprenticeships.NewDB.Domain.Entities.Vacancy;


    public class NewDBDatabaseRespository : INewDBRepository
    {
        public Func<IDbConnection> _getOpenConnection;

        public NewDBDatabaseRespository(Func<IDbConnection> getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public int AddVacancy(Vacancy.Vacancy vacancy)
        {
            using (var conn = _getOpenConnection())
            {
                return conn.Query<int>(@"
INSERT INTO Vacancy.Vacancy
       ( EmployerVacancyPartyId,  VacancyReferenceNumber,  VacancyTypeCode,  VacancyStatusCode,  VacancyLocationTypeCode,  OwnerVacancyPartyId,  ContractOwnerVacancyPartyId,  ManagerVacancyPartyId,  DeliveryProviderVacancyPartyId,  Title,  TrainingTypeCode,  FrameworkId,  FrameworkIdComment,  StandardId,  LevelCode)
VALUES (@EmployerVacancyPartyId, @VacancyReferenceNumber, @VacancyTypeCode, @VacancyStatusCode, @VacancyLocationTypeCode, @OwnerVacancyPartyId, @ContractOwnerVacancyPartyId, @ManagerVacancyPartyId, @DeliveryProviderVacancyPartyId, @Title, @TrainingTypeCode, @FrameworkId, @FrameworkIdComment, @StandardId, @LevelCode)
;
SELECT CAST(SCOPE_IDENTITY() AS INT)
", vacancy).Single();
            }
        }
        /*
        OwnerVacancyPartyId = 42,
                ContractOwnerVacancyPartyId = 42,
                EmployerVacancyPartyId = 42,
                ManagerVacancyPartyId = 42,
                */

        public Vacancy.Vacancy GetVacancy(int vacancyId)
        {
            using (var conn = _getOpenConnection())
            {
                return conn.Query<Vacancy.Vacancy>(@"
SELECT *
FROM   Vacancy.Vacancy
WHERE  VacancyId = @VacancyId
", new { VacancyId = vacancyId }, buffered: false).Single();
            }
        }
    }
}
