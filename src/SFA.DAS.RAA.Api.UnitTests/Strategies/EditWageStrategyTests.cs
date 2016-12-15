namespace SFA.DAS.RAA.Api.UnitTests.Strategies
{
    using System;
    using Api.Strategies;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Domain.Entities.Vacancies;
    using FluentAssertions;
    using Models;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Providers;

    [TestFixture]
    [Parallelizable]
    public class EditWageStrategyTests
    {
        [TestCase(VacancyStatus.Draft)]
        [TestCase(VacancyStatus.Unknown)]
        [TestCase(VacancyStatus.Draft)]
        [TestCase(VacancyStatus.Referred)]
        [TestCase(VacancyStatus.Deleted)]
        [TestCase(VacancyStatus.Submitted)]
        [TestCase(VacancyStatus.Withdrawn)]
        [TestCase(VacancyStatus.Completed)]
        [TestCase(VacancyStatus.PostedInError)]
        [TestCase(VacancyStatus.ReservedForQA)]
        public void EditWageInvalidVacancyStatus(VacancyStatus vacancyStatus)
        {
            //Arrange
            const int vacancyId = 1;
            var vacancy = new Fixture().Build<Vacancy>()
                .With(v => v.VacancyId, vacancyId)
                .With(v => v.Status, vacancyStatus)
                .Create();
            var vacancyProvider = new Mock<IVacancyProvider>();
            vacancyProvider.Setup(p => p.Get(vacancyId, null, null)).Returns(vacancy);
            var strategy = new EditWageStrategy(vacancyProvider.Object);

            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = 220,
                AmountLowerBound = 200,
                AmountUpperBound = 240,
                Unit = WageUnit.Weekly
            };

            //Act
            Action editWageAction = () => strategy.EditWage(wageUpdate, vacancyId);

            //Assert
            editWageAction.ShouldThrow<ArgumentException>().WithMessage("You can only edit the wage of a vacancy that is live or closed.");
        }

        [TestCase(VacancyStatus.Live)]
        [TestCase(VacancyStatus.Closed)]
        public void EditWageUpdatesVacancy(VacancyStatus vacancyStatus)
        {
            //Arrange
            const int vacancyId = 1;
            var vacancy = new Fixture().Build<Vacancy>()
                .With(v => v.VacancyId, vacancyId)
                .With(v => v.Status, vacancyStatus)
                .Create();
            var vacancyProvider = new Mock<IVacancyProvider>();
            vacancyProvider.Setup(p => p.Get(vacancyId, null, null)).Returns(vacancy);
            var strategy = new EditWageStrategy(vacancyProvider.Object);

            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = 220,
                AmountLowerBound = 200,
                AmountUpperBound = 240,
                Unit = WageUnit.Weekly
            };

            //Act
            var updatedVacancy = strategy.EditWage(wageUpdate, vacancyId);

            //Assert
            updatedVacancy.Should().NotBeNull();
            var updatedWage = updatedVacancy.Wage;
            updatedWage.Should().NotBeNull();
            updatedWage.Type.Should().Be(wageUpdate.Type);
            updatedWage.Amount.Should().Be(wageUpdate.Amount);
            updatedWage.AmountLowerBound.Should().Be(wageUpdate.Amount);
            updatedWage.AmountUpperBound.Should().Be(wageUpdate.Amount);
            updatedWage.Unit.Should().Be(wageUpdate.Unit);
        }
    }
}