namespace SFA.Apprenticeships.Infrastructure.UnitTests.Presentation
{
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;

    [TestFixture]
    public class WagePresenterTests
    {
        [TestCase(WageType.LegacyWeekly)]
        public void ShouldGetHeaderDisplayText(WageType wageType)
        {
            // Arrange.
            var wage = new Wage(wageType, null, WageUnit.Weekly);

            // Act.
            var actual = wage.GetHeaderDisplayText();

            // Assert.
            actual.Should().Be(WagePresenter.WeeklyWageText);
        }
    }
}
