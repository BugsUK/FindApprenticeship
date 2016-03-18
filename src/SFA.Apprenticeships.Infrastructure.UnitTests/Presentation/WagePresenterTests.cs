namespace SFA.Apprenticeships.Infrastructure.UnitTests.Presentation
{
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;

    [TestFixture]
    public class WagePresenterTests
    {
        [TestCase(WageUnit.Weekly, WagePresenter.WeeklyWageText)]
        [TestCase(WageUnit.Annually, WagePresenter.AnnualWageText)]
        public void ShouldGetHeaderDisplayText(WageUnit wageUnit, string expected)
        {
            // Arrange.
            var wage = new Wage(WageType.Custom, null, WageUnit.Weekly);

            // Act.
            var actual = wage.GetHeaderDisplayText();

            // Assert.
            actual.Should().Be(expected);
        }
    }
}
