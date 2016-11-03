namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Vacancies
{
    using Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using NUnit.Framework.Internal;

    [TestFixture]
    public class WageTests
    {
        [TestCase(WageType.LegacyWeekly, WageUnit.NotApplicable, WageUnit.Weekly)]
        [TestCase(WageType.LegacyWeekly, WageUnit.Weekly, WageUnit.Weekly)]
        [TestCase(WageType.LegacyWeekly, WageUnit.Monthly, WageUnit.Weekly)]
        [TestCase(WageType.LegacyWeekly, WageUnit.Annually, WageUnit.Weekly)]
        [TestCase(WageType.LegacyText, WageUnit.NotApplicable, WageUnit.NotApplicable)]
        [TestCase(WageType.LegacyText, WageUnit.Weekly, WageUnit.NotApplicable)]
        [TestCase(WageType.LegacyText, WageUnit.Monthly, WageUnit.NotApplicable)]
        [TestCase(WageType.LegacyText, WageUnit.Annually, WageUnit.NotApplicable)]
        [TestCase(WageType.Custom, WageUnit.NotApplicable, WageUnit.NotApplicable)]
        [TestCase(WageType.Custom, WageUnit.Weekly, WageUnit.Weekly)]
        [TestCase(WageType.Custom, WageUnit.Monthly, WageUnit.Monthly)]
        [TestCase(WageType.Custom, WageUnit.Annually, WageUnit.Annually)]
        [TestCase(WageType.ApprenticeshipMinimum, WageUnit.NotApplicable, WageUnit.Weekly)]
        [TestCase(WageType.ApprenticeshipMinimum, WageUnit.Weekly, WageUnit.Weekly)]
        [TestCase(WageType.ApprenticeshipMinimum, WageUnit.Monthly, WageUnit.Weekly)]
        [TestCase(WageType.ApprenticeshipMinimum, WageUnit.Annually, WageUnit.Weekly)]
        [TestCase(WageType.NationalMinimum, WageUnit.NotApplicable, WageUnit.Weekly)]
        [TestCase(WageType.NationalMinimum, WageUnit.Weekly, WageUnit.Weekly)]
        [TestCase(WageType.NationalMinimum, WageUnit.Monthly, WageUnit.Weekly)]
        [TestCase(WageType.NationalMinimum, WageUnit.Annually, WageUnit.Weekly)]
        [TestCase(WageType.CustomRange, WageUnit.NotApplicable, WageUnit.Weekly)]
        [TestCase(WageType.CustomRange, WageUnit.Annually, WageUnit.Annually)]
        [TestCase(WageType.CustomRange, WageUnit.Monthly, WageUnit.Monthly)]
        [TestCase(WageType.CustomRange, WageUnit.Weekly, WageUnit.Weekly)]
        [TestCase(WageType.CompetitiveSalary, WageUnit.Weekly, WageUnit.NotApplicable)]
        [TestCase(WageType.CompetitiveSalary, WageUnit.Monthly, WageUnit.NotApplicable)]
        [TestCase(WageType.CompetitiveSalary, WageUnit.Annually, WageUnit.NotApplicable)]
        [TestCase(WageType.CompetitiveSalary, WageUnit.NotApplicable, WageUnit.NotApplicable)]
        [TestCase(WageType.ToBeAgreedUponAppointment, WageUnit.Weekly, WageUnit.NotApplicable)]
        [TestCase(WageType.ToBeAgreedUponAppointment, WageUnit.Monthly, WageUnit.NotApplicable)]
        [TestCase(WageType.ToBeAgreedUponAppointment, WageUnit.Annually, WageUnit.NotApplicable)]
        [TestCase(WageType.ToBeAgreedUponAppointment, WageUnit.NotApplicable, WageUnit.NotApplicable)]
        [TestCase(WageType.Unwaged, WageUnit.Weekly, WageUnit.NotApplicable)]
        [TestCase(WageType.Unwaged, WageUnit.Monthly, WageUnit.NotApplicable)]
        [TestCase(WageType.Unwaged, WageUnit.Annually, WageUnit.NotApplicable)]
        [TestCase(WageType.Unwaged, WageUnit.NotApplicable, WageUnit.NotApplicable)]
        public void ShouldSetCorrectWageUnit(WageType type, WageUnit suppliedUnit, WageUnit expectedUnit)
        {
            //Arrange
            //Act
            var result = new Wage(type, null, null, null, null, suppliedUnit, null, null);

            //Assert
            result.Unit.Should().Be(expectedUnit);
        }
    }
}
