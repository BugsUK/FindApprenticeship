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

        [TestCase(WageUnit.Weekly, 5, "£5.00" + Space + WagePresenter.PerWeekText)]
        [TestCase(WageUnit.Monthly, 5, "£5.00" + Space + WagePresenter.PerMonthText)]
        [TestCase(WageUnit.Annually, 5, "£5.00" + Space + WagePresenter.PerYearText)]
        [TestCase(WageUnit.NotApplicable, 5, "£5.00" + Space)]
        public void ShouldGetDisplayAmountWithFrequencyPostfix(WageUnit wageUnit, decimal displayAmount, string expected)
        {
            // Act.
            var actual = WagePresenter.GetDisplayAmountWithFrequencyPostfix(WageType.Custom, displayAmount, null, null, null, wageUnit, null, null);

            // Assert.
            actual.Should().Be(expected);
        }

        [TestCase(WageType.ApprenticeshipMinimum, "£123.75" + Space + WagePresenter.PerWeekText)]
        [TestCase(WageType.NationalMinimum, "£145.13 - £251.25" + Space + WagePresenter.PerWeekText)]
        public void ShouldGetDisplayAmountWithFrequencyPostfixNationalMinimums(WageType wageType, string expected)
        {
            // Act.
            var actual = WagePresenter.GetDisplayAmountWithFrequencyPostfix(wageType, null, null, null, null, WageUnit.Weekly, 37.5m, new DateTime(2016, 9, 30));

            // Assert.
            actual.Should().Be(expected);
        }

        [TestCase(WageType.ApprenticeshipMinimum, "£127.50" + Space + WagePresenter.PerWeekText)]
        [TestCase(WageType.NationalMinimum, "£150.00 - £260.63" + Space + WagePresenter.PerWeekText)]
        public void ShouldGetDisplayAmountWithFrequencyPostfixNationalMinimums_After1stOct2016(WageType wageType, string expected)
        {
            // Act.
            var actual = WagePresenter.GetDisplayAmountWithFrequencyPostfix(wageType, null, null, null, null, WageUnit.Weekly, 37.5m, new DateTime(2016, 10, 1));

            // Assert.
            actual.Should().Be(expected);
        }
        
        [TestCase(WageType.LegacyText, "Competitive salary", "123.45", null, null, null, "Competitive salary")]
        [TestCase(WageType.LegacyText, null, "123.45", null, null, null, WagePresenter.UnknownText)]
        [TestCase(WageType.CustomRange, null, null, "1", "2", null, @"£1.00 - £2.00")]
        [TestCase(WageType.CustomRange, null, null, null, "2", null, @"£unknown - £2.00")]
        [TestCase(WageType.CustomRange, null, null, "1", null, null, @"£1.00 - £unknown")]
        [TestCase(WageType.CompetitiveSalary, null, null, null, null, null, WagePresenter.CompetitiveSalaryText)]
        [TestCase(WageType.ToBeAgreedUponAppointment, null, null, null, null, null, WagePresenter.ToBeAGreedUponAppointmentText)]
        [TestCase(WageType.Unwaged, null, null, null, null, null, WagePresenter.UnwagedText)]
        public void ShouldGetDisplayText(
            WageType wageType, string wageText, string wageAmountString, string wageLowerString, string wageUpperString, string hoursPerWeekString, string expected)
        {
            // Arrange.  This is frustrating. You cant use decimal parameters as decimals aren't primitives.
            decimal tempDecimal;
            decimal? wageAmount = null;
            decimal? wageAmountLower = null;
            decimal? wageAmountUpper = null;
            decimal? hoursPerWeek = null;

            if (decimal.TryParse(wageAmountString, out tempDecimal))
            {
                wageAmount = tempDecimal;
            }

            if (decimal.TryParse(wageLowerString, out tempDecimal))
            {
                wageAmountLower = tempDecimal;
            }

            if (decimal.TryParse(wageUpperString, out tempDecimal))
            {
                wageAmountUpper = tempDecimal;
            }

            if (decimal.TryParse(hoursPerWeekString, out tempDecimal))
            {
                hoursPerWeek = tempDecimal;
            }

            // Act.
            var actual = WagePresenter.GetDisplayAmount(wageType, wageAmount, wageAmountLower, wageAmountUpper, wageText, Convert.ToDecimal(hoursPerWeek), null);

            // Assert.
            actual.Should().Be(expected);
        }

        [TestCase(WageType.LegacyWeekly, WageUnit.Weekly)]
        [TestCase(WageType.LegacyText, WageUnit.NotApplicable)]
        [TestCase(WageType.ApprenticeshipMinimum, WageUnit.Weekly)]
        [TestCase(WageType.NationalMinimum, WageUnit.Weekly)]
        [TestCase(WageType.CompetitiveSalary, WageUnit.NotApplicable)]
        [TestCase(WageType.ToBeAgreedUponAppointment, WageUnit.NotApplicable)]
        [TestCase(WageType.Unwaged, WageUnit.NotApplicable)]
        public void ShouldGetTheCorrectWageUnitForNonCustomWages(WageType wageType, WageUnit expected)
        {
            var wage = new Wage(wageType, null, null, null, string.Empty, WageUnit.NotApplicable, null, null);
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
            var wage = new Wage(WageType.Custom, null, null, null, string.Empty, wageUnit, null, null);

            var actual = wage.Unit;

            actual.Should().Be(expected);
        }
    }
}
