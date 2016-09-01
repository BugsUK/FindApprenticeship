namespace SFA.Apprenticeships.Infrastructure.UnitTests.VacancySearch.Mappers
{
    using Infrastructure.VacancySearch.Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class VacancySearchMapperTests
    {
        [Test]
        public void ShouldCreateMap()
        {
            new VacancySearchMapper().Mapper.AssertConfigurationIsValid();
        }
    }
}