namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.UserProfile
{
    using NUnit.Framework;
    using Schemas.UserProfile;

    [TestFixture]
    public class AgencyUserMappersTests
    {
        [Test]
        public void DoMappersMapEverything()
        {
            // Arrange
            var x = new AgencyUserMappers();

            // Act
            x.Initialise();

            // Assert
            x.Mapper.AssertConfigurationIsValid();
        }
    }
}
