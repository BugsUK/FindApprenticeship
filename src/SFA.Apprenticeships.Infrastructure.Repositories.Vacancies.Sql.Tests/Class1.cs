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
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Repositories;
    using Moq;
    using SFA.Infrastructure.Interfaces;
    using SFA.Infrastructure.Sql;

    [TestFixture]
    public class Class1
    {
        private const string ConnectionString = "Server=VTUK027\\SQLEXPRESS;Database=Raa2;Trusted_Connection=True;"; //TODO: get from settings
        readonly IMapper _mapper = new ApprenticeshipVacancyMappers();

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            var solutionDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\..\\..\\..\\SFA.Apprenticeships.Data";
            var databaseName = "Raa2";
            var dbInitialiser = new DatabaseInitialiser(solutionDirectory, ConnectionString, databaseName);
            var seedScripts = new[]
            {
                AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\reference.occupation.sql",
                AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\reference.framework.sql",
                AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\vacancy.vacancyparty.sql",
                AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\vacancy.insertDraftVacancy.sql"
            };
             dbInitialiser.Publish(true, seedScripts);
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
