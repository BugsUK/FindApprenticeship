namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Employer
{
    using NUnit.Framework;
    using Schemas.dbo;

    [TestFixture]
    public class EmployerMappersTests
    {
        [Test]
        public void DoMappersMapEverything()
        {
            // Arrange
            var mapper = new EmployerMappers();

            // Act
            mapper.Initialise();

            // Assert
            mapper.Mapper.AssertConfigurationIsValid();
        }
    }
}