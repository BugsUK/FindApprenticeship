namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa.Extensions.VacancyExtensions
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Infrastructure.Raa.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyExtensionsTests
    {
        [TestCase(VacancyStatus.Unknown, VacancyStatuses.Unknown)]
        [TestCase(VacancyStatus.Draft, VacancyStatuses.Unavailable)]
        [TestCase(VacancyStatus.Submitted, VacancyStatuses.Unavailable)]
        [TestCase(VacancyStatus.Live, VacancyStatuses.Live)]
        [TestCase(VacancyStatus.ReservedForQA, VacancyStatuses.Unavailable)]
        [TestCase(VacancyStatus.Referred, VacancyStatuses.Unavailable)]
        [TestCase(VacancyStatus.Closed, VacancyStatuses.Expired)]
        public void ShouldMapVacancyStatus(VacancyStatus fromVacancyStatus, VacancyStatuses expectedVacancyStatus)
        {
            // Act.
            var actualVacancyStatus = fromVacancyStatus.GetVacancyStatuses();

            // Assert.
            actualVacancyStatus.Should().Be(expectedVacancyStatus);
        }

        [Test]
        public void ShouldThrowIfUnhandledVacancyStatus()
        {
            const VacancyStatus unhandledVacancyStatus = VacancyStatus.PostedInError;

            // Act.
            Action action = () => unhandledVacancyStatus.GetVacancyStatuses();

            // Assert.
            action.ShouldThrow<ArgumentException>();
        }
    }
}
