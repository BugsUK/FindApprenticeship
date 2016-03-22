namespace SFA.Apprenticeships.Infrastructure.UnitTests.Presentation
{
    using System;
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
            var wage = new Wage(WageType.Custom, null, null, wageUnit);

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
            var wage = new Wage(wageType, null, null, WageUnit.NotApplicable);

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

        [TestCase(WageType.LegacyText, "Competitive salary", "123.45", null, "Competitive salary")]
        [TestCase(WageType.LegacyText, null, "123.45", null, WagePresenter.UnknownText)]
        public void ShouldGetDisplayText(
            WageType wageType, string wageText, string wageAmountString, string hoursPerWeekString, string expected)
        {
            // Arrange.
            decimal tempDecimal;
            decimal? wageAmount = null;
            decimal? hoursPerWeek = null;

            if (decimal.TryParse(wageAmountString, out tempDecimal))
            {
                wageAmount = tempDecimal;
            }

            if (decimal.TryParse(hoursPerWeekString, out tempDecimal))
            {
                hoursPerWeek = tempDecimal;
            }

            var wage = new Wage(wageType, wageAmount, wageText, WageUnit.NotApplicable);

            // Act.
            var actual = wage.GetDisplayText(Convert.ToDecimal(hoursPerWeek));

            // Assert.
            actual.Should().Be(expected);
        }
    }
}
