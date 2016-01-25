namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using AvService.Mappers.Version51;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyDurationMapperTests
    {
        private IVacancyDurationMapper _vacancyDurationMapper;

        [SetUp]
        public void SetUp()
        {
            _vacancyDurationMapper = new VacancyDurationMapper();
        }

        [TestCase(1, DurationType.Weeks, "1 week")]
        [TestCase(64, DurationType.Weeks, "64 weeks")]
        [TestCase(1, DurationType.Months, "1 month")]
        [TestCase(12, DurationType.Months, "12 months")]
        [TestCase(1, DurationType.Years, "1 year")]
        [TestCase(2, DurationType.Years, "2 years")]
        public void ShouldMapDurationToString(int duration, DurationType durationType, string expectedDurationString)
        {
            // Act.
            var durationString = _vacancyDurationMapper.MapDurationToString(duration, durationType);

            // Assert.
            durationString.Should().Be(expectedDurationString);
        }

        [Test]
        public void ShouldMapToEmptyStringIfDurationIsNull()
        {
            // Act.
            var durationString = _vacancyDurationMapper.MapDurationToString(null, DurationType.Weeks);

            // Assert.
            durationString.Should().Be(string.Empty);
        }

        [Test]
        public void ShouldMapToEmptyStringIfDurationTypeIsUnknown()
        {
            // Act.
            var durationString = _vacancyDurationMapper.MapDurationToString(1, DurationType.Unknown);

            // Assert.
            durationString.Should().Be(string.Empty);
        }
    }
}
