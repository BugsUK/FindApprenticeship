using NUnit.Framework;
using SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests
{
    using System.Data.SqlClient;
    using System.IO;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NewDB.Domain.Entities;
    using SFA.Infrastructure.Interfaces;
    using SFA.Infrastructure.Sql;
    using Vacancy = NewDB.Domain.Entities.Vacancy;

    [TestFixture]
    public class Class1
    {
        private const string ConnectionString = "Server=VTUK027\\SQLEXPRESS;Database=Raa2;Trusted_Connection=True;"; //TODO: get from settings
        readonly IMapper _mapper = new ApprenticeshipVacancyMappers();
        private const int VacancyReferenceNumber = 1;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            var databaseProjectName = "SFA.Apprenticeships.Data";
            var databaseProjectPath = AppDomain.CurrentDomain.BaseDirectory + $"\\..\\..\\..\\{databaseProjectName}";
            var databaseName = "Raa2";
            var dacpacFilePath = Path.Combine(databaseProjectPath + $"\\bin\\Local\\{databaseProjectName}.dacpac"); //TODO get configuration from settings
            var dbInitialiser = new DatabaseInitialiser(dacpacFilePath, ConnectionString, databaseName);
            var seedScripts = new[]
            {
                AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\reference.occupation.sql",
                AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\reference.framework.sql",
                AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\vacancy.vacancyparty.sql",
                AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\vacancy.insertDraftVacancy.sql"
            };
             
            dbInitialiser.Publish(true);
            //dbInitialiser.Seed(seedScripts);

            var vacancy = new Vacancy.Vacancy
            {
                VacancyReferenceNumber = VacancyReferenceNumber,
                AV_ContactName = "av contact name",
                VacancyTypeCode = "A",
                VacancyStatusCode = "LIV",
                VacancyLocationTypeCode = "S",
                Title = "Test vacancy",
                TrainingTypeCode = "F",
                LevelCode = "4",
                FrameworkId = 1,
                WageValue = 100.0M,
                ClosingDate = DateTime.Now,
                ContractOwnerVacancyPartyId = 1,
                DeliveryProviderVacancyPartyId = 1,
                EmployerVacancyPartyId = 1,
                ManagerVacancyPartyId = 1,
                OriginalContractOwnerVacancyPartyId = 1,
                ParentVacancyId = null,
                OwnerVacancyPartyId = 1,
                DurationValue = 3
            };

            var occupation = new Occupation
            {
                OccupationId = 1,
                OccupationStatusId = 1,
                CodeName = "O01",
                FullName = "Occupation 1",
                ShortName = "Occupation 1"
            };

            var framework = new Framework
            {
                FrameworkId = 1,
                CodeName = "F01",
                FullName = "Framework 1",
                ShortName = "Framework 1",
                FrameworkStatusId = 1,
                OccupationId = 1
            };

            var vacancyParty1 = new Vacancy.VacancyParty
            {
                VacancyPartyTypeCode = "ES",
                FullName = "Employer A",
                Description = "A",
                WebsiteUrl = "URL",
                EDSURN = 1,
                UKPRN = null
            };

            var vacancyParty2 = new Vacancy.VacancyParty
            {
                VacancyPartyTypeCode = "PS",
                FullName = "Provider A",
                Description = "A",
                WebsiteUrl = "URL",
                EDSURN = null,
                UKPRN = 1
            };

            dbInitialiser.Seed(new object[] {occupation, framework, vacancyParty1, vacancyParty2, vacancy });
        }

        [Test]
        public void GetVacancyTestByGuid()
        {
            var vacancyGuid = Guid.Empty;
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                var commandText = $"SELECT [VacancyId] FROM [Vacancy].[Vacancy] WHERE [VacancyReferenceNumber] = {VacancyReferenceNumber}";
                using (var command = new SqlCommand(commandText, sqlConnection))
                {
                    vacancyGuid = (Guid)command.ExecuteScalar();
                }

                sqlConnection.Close();
            }

            // configure _mapper
            IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(ConnectionString);
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(connection, _mapper, logger.Object);

            var vacancy = repository.Get(vacancyGuid);
        }

        [Test]
        public void GetVacancyTest()
        {
            // configure _mapper
            IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(ConnectionString);
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(connection, _mapper, logger.Object);

            var vacancy = repository.Get(1);
        }

        [Test]
        public void ReserveVacancyForQaTest()
        {
            IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(ConnectionString);
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyWriteRepository repository = new ApprenticeshipVacancyRepository(connection, _mapper, logger.Object);

            repository.ReserveVacancyForQA(1);

            var vacancy = GetVacancy(1L);
        }

        private static ApprenticeshipVacancy GetVacancy(long vacancyReferenceNumber)
        {
            ApprenticeshipVacancy vacancy;

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var commandText = $"SELECT * FROM Vacancy.Vacancy WHERE VacancyReferenceNumber = {vacancyReferenceNumber}";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var pp = reader[3].ToString();

                        vacancy = new ApprenticeshipVacancy
                        {
                            Status =
                                (ProviderVacancyStatuses)
                                    Enum.Parse(typeof (ProviderVacancyStatuses), reader[3].ToString())
                        };

                    }
                }
                connection.Close();
            }

            return vacancy;
        }

        [Test]
        public void DoMappersMapEverything()
        {
            // Arrange
            var x = new ApprenticeshipVacancyMappers();

            // Act
            x.Initialise();

            // Assert
            x.Mapper.AssertConfigurationIsValid();
        }
    }
}
