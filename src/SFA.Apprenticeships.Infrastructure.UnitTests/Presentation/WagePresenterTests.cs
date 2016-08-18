namespace SFA.Apprenticeships.Infrastructure.UnitTests.Presentation
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class WagePresenterTests
    {
        private const string Space = " ";

        [TestCase(WageUnit.Weekly, WagePresenter.WeeklyWageText)]
        [TestCase(WageUnit.Annually, WagePresenter.AnnualWageText)]
        [TestCase(WageUnit.Monthly, WagePresenter.MonthlyWageText)]
        [TestCase(WageUnit.NotApplicable, "")]
        public void ShouldGetHeaderDisplayTextForCustomWageType(WageUnit wageUnit, string expected)
        {
            // Arrange.

            // Act.
            var actual = wageUnit.GetHeaderDisplayText();

            // Assert.
            actual.Should().Be(expected);
        }

        [TestCase(WageUnit.Weekly, WagePresenter.WeeklyWageText)]
        [TestCase(WageUnit.Monthly, WagePresenter.MonthlyWageText)]
        [TestCase(WageUnit.Annually, WagePresenter.AnnualWageText)]
        [TestCase(WageUnit.NotApplicable, "")]
        public void ShouldGetHeaderDisplayTextForWageUnit(WageUnit wageUnit, string expected)
        {
            // Act.
            var actual = wageUnit.GetHeaderDisplayText();

            // Assert.
            actual.Should().Be(expected);
        }

        [TestCase(WageUnit.Weekly, "5", "5" + Space + WagePresenter.PerWeekText)]
        [TestCase(WageUnit.Monthly, "5", "5" + Space + WagePresenter.PerMonthText)]
        [TestCase(WageUnit.Annually, "5", "5" + Space + WagePresenter.PerYearText)]
        [TestCase(WageUnit.NotApplicable, "5", "5" + Space)]
        public void ShouldGetDisplayAmountWithFrequencyPostfix(WageUnit wageUnit, string displayAmount, string expected)
        {
            // Act.
            var actual = wageUnit.GetDisplayAmountWithFrequencyPostfix(displayAmount);

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

            var wage = new Wage(wageType, wageAmount, wageText, WageUnit.NotApplicable, Convert.ToDecimal(hoursPerWeek));

            // Act.
            var actual = wage.GetDisplayAmount();

            // Assert.
            actual.Should().Be(expected);
        }

        [TestCase(WageType.LegacyWeekly, WageUnit.Weekly)]
        [TestCase(WageType.LegacyText, WageUnit.NotApplicable)]
        [TestCase(WageType.ApprenticeshipMinimum, WageUnit.Weekly)]
        [TestCase(WageType.NationalMinimum, WageUnit.Weekly)]
        public void ShouldGetTheCorrectWageUnitForNonCustomWages(WageType wageType, WageUnit expected)
        {
            var wage = new Wage(wageType, null, string.Empty, WageUnit.NotApplicable);
            // Act.
            var actual = wage.Unit;

            // Assert.
            actual.Should().Be(expected);
        }

        [TestCase(WageUnit.Weekly, WageUnit.Weekly)]
        [TestCase(WageUnit.Monthly, WageUnit.Monthly)]
        [TestCase(WageUnit.Annually, WageUnit.Annually)]
        [TestCase(WageUnit.NotApplicable, WageUnit.NotApplicable)]
        public void ShouldGetTheCorrectWageUnitForCustomWages(WageUnit wageUnit, WageUnit expected)
        {
            var wage = new Wage(WageType.Custom, null, string.Empty, wageUnit);

            var actual = wage.Unit;

            actual.Should().Be(expected);
        }
    }
}
