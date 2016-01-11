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
