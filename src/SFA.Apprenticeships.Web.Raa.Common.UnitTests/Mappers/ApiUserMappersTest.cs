namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using Common.Mappers;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ApiUserMappersTest
    {
        [Test]
        public void ShouldCreateMap()
        {
            new ApiUserMappers().Mapper.AssertConfigurationIsValid();
        }
    }
}