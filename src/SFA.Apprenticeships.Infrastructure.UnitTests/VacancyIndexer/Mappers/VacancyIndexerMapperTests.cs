namespace SFA.Apprenticeships.Infrastructure.UnitTests.VacancyIndexer.Mappers
{
    using Infrastructure.VacancyIndexer.Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyIndexerMapperTests
    {
        [Test]
        public void ShouldCreateMap()
        {
            new VacancyIndexerMapper().Mapper.AssertConfigurationIsValid();
        }
    }
}