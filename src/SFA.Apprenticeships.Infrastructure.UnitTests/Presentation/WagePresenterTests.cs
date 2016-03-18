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
        [TestCase(WageUnit.Monthly, WagePresenter.MonthlyWageText)]
        [TestCase(WageUnit.NotApplicable, "")]
        public void ShouldGetHeaderDisplayTextForCustomWageType(WageUnit wageUnit, string expected)
        {
            // Arrange.
            var wage = new Wage(WageType.Custom, null, wageUnit);

            // Act.
            var actual = wage.GetHeaderDisplayText();

            // Assert.
            actual.Should().Be(expected);
        }

        [TestCase(WageType.LegacyText)]
        [TestCase(WageType.LegacyWeekly)]
        [TestCase(WageType.ApprenticeshipMinimum)]
        [TestCase(WageType.NationalMinimum)]
        public void ShouldGetHeaderDisplayTextForNonCustomWageTypes(WageType wageType)
        {
            // Arrange.
            var wage = new Wage(wageType, null, WageUnit.NotApplicable);

            // Act.
            var actual = wage.GetHeaderDisplayText();

            // Assert.
            actual.Should().Be(WagePresenter.WeeklyWageText);
        }

        [TestCase(Domain.Entities.Vacancies.WageUnit.Weekly, WagePresenter.WeeklyWageText)]
        [TestCase(Domain.Entities.Vacancies.WageUnit.Monthly, WagePresenter.MonthlyWageText)]
        [TestCase(Domain.Entities.Vacancies.WageUnit.Annually, WagePresenter.AnnualWageText)]
        [TestCase(Domain.Entities.Vacancies.WageUnit.NotApplicable, "")]
        public void ShouldGetHeaderDisplayTextForWageUnit(Domain.Entities.Vacancies.WageUnit wageUnit, string expected)
        {
            // Act.
            var actual = wageUnit.GetHeaderDisplayText();

            // Assert.
            actual.Should().Be(expected);
        }

        [TestCase(Domain.Entities.Vacancies.WageUnit.Weekly, WagePresenter.PerWeekText)]
        [TestCase(Domain.Entities.Vacancies.WageUnit.Monthly, WagePresenter.PerMonthText)]
        [TestCase(Domain.Entities.Vacancies.WageUnit.Annually, WagePresenter.PerYearText)]
        [TestCase(Domain.Entities.Vacancies.WageUnit.NotApplicable, "")]
        public void ShouldGetWagePostfix(Domain.Entities.Vacancies.WageUnit wageUnit, string expected)
        {
            // Act.
            var actual = wageUnit.GetWagePostfix();

            // Assert.
            actual.Should().Be(expected);
        }
    }
}
