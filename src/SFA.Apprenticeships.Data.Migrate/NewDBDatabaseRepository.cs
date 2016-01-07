namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Linq;

    using Vacancy = SFA.Apprenticeships.NewDB.Domain.Entities.Vacancy;
    using Infrastructure.Sql;

    public class NewDBDatabaseRespository : INewDBRepository
    {
        public IGetOpenConnection _getOpenConnection;

        public NewDBDatabaseRespository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public int AddVacancy(Vacancy.Vacancy vacancy)
        {
            return _getOpenConnection.Query<int>(@"
INSERT INTO Vacancy.Vacancy
       ( EmployerVacancyPartyId,  VacancyReferenceNumber,  VacancyTypeCode,  VacancyStatusCode,  VacancyLocationTypeCode,  OwnerVacancyPartyId,  ContractOwnerVacancyPartyId,  ManagerVacancyPartyId,  DeliveryProviderVacancyPartyId,  Title,  TrainingTypeCode,  FrameworkId,  FrameworkIdComment,  StandardId,  LevelCode)
VALUES (@EmployerVacancyPartyId, @VacancyReferenceNumber, @VacancyTypeCode, @VacancyStatusCode, @VacancyLocationTypeCode, @OwnerVacancyPartyId, @ContractOwnerVacancyPartyId, @ManagerVacancyPartyId, @DeliveryProviderVacancyPartyId, @Title, @TrainingTypeCode, @FrameworkId, @FrameworkIdComment, @StandardId, @LevelCode)
;
SELECT CAST(SCOPE_IDENTITY() AS INT)
", vacancy).Single();
        }

        public Vacancy.Vacancy GetVacancy(int vacancyId)
        {
            return _getOpenConnection.Query<Vacancy.Vacancy>(@"
SELECT *
FROM   Vacancy.Vacancy
WHERE  VacancyId = @VacancyId
", new { VacancyId = vacancyId }).Single();
        }
    }
}
