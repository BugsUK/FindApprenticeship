namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers
{
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using AvService.Mappers.Version51;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class WageMapperTests
    {
        [TestCase(WageType.Custom, WageUnit.Weekly, "0", "£0.00")]
        [TestCase(WageType.Custom, WageUnit.Weekly, "123.45", "£123.45")]
        [TestCase(WageType.Custom, WageUnit.Weekly, "123.454", "£123.45")]
        [TestCase(WageType.Custom, WageUnit.Weekly, "123.456", "£123.46")]
        public void ShouldMapCustomWageToWageText(
            WageType wageType, WageUnit wageUnit, string wageAsDecimalString, string expectedWageText)
        {
            // Arrange.
            decimal wage;
            decimal.TryParse(wageAsDecimalString, out wage);

            // Act.
            var mappedWageText = WageMapper.MapToText(wageType, wageUnit, wage);

            // Assert.
            mappedWageText.Should().Be(expectedWageText);
        }
    }
}
