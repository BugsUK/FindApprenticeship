namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.VacancyParty
{
    using NUnit.Framework;
    using Schemas.dbo;

    [TestFixture]
    [Parallelizable]
    public class VacancyPartyMappersTests
    {
        [Test]
        public void DoMappersMapEverything()
        {
            // Arrange
            var mapper = new VacancyPartyMappers();

            // Act
            mapper.Initialise();

            // Assert
            mapper.Mapper.AssertConfigurationIsValid();
        }
    }
}