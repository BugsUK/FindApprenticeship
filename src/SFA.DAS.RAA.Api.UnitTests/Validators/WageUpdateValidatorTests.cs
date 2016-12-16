namespace SFA.DAS.RAA.Api.UnitTests.Validators
{
    using Api.Validators;
    using Apprenticeships.Domain.Entities.Vacancies;
    using FluentAssertions;
    using FluentValidation;
    using Models;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class WageUpdateValidatorTests
    {
        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, false)]
        [TestCase(WageType.NationalMinimum, false)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.CustomRange, true)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidFixedWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.Custom,
                ExistingAmount = 100,
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
        }

        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, false)]
        [TestCase(WageType.NationalMinimum, false)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.CustomRange, true)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidWageRangeTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.CustomRange,
                ExistingAmount = 100,
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
        }
    }
}