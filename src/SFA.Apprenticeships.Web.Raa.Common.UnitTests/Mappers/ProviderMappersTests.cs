namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using Common.Mappers;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ProviderMappersTests
    {
        [Test]
        public void ShouldCreateMap()
        {
            new ProviderMappers().Mapper.AssertConfigurationIsValid();
        }
    }
}