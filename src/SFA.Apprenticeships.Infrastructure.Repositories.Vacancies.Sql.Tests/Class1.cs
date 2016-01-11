using NUnit.Framework;
using SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests
{
    [TestFixture]
    public class Class1
    {
        [SetUp]
        public void SetUp()
        {
            var solutionDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\..\\..\\..\\SFA.Apprenticeships.Data";
            var connectionString = "Server=VTUK027\\SQLEXPRESS;Database=Raa2;Trusted_Connection=True;"; //TODO: get from settings
            var databaseName = "Raa2";
            var dbInitialiser = new DatabaseInitialiser(solutionDirectory, connectionString, databaseName);
            dbInitialiser.Publish(true);    
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
