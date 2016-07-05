namespace SFA.Apprenticeships.Application.UnitTests.Applications.Extensions
{
    using System;
    using Apprenticeships.Application.Applications.Entities;
    using Apprenticeships.Application.Applications.Extensions;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ApprenticeshipApplicationDetailExtensionTests
    {
        public class WhenLegacySystemUpdate
        {
            private Mock<IApprenticeshipApplicationWriteRepository> _repository;

            [SetUp]
            public void SetUp()
            {
                _repository = new Mock<IApprenticeshipApplicationWriteRepository>();
            }

            [TestCase(1, 2, true)]
            [TestCase(5, 5, false)]
            public void ShouldUpdateLegacyApplicationId(int oldId, int newId, bool expected)
            {
                // Arrange.
                var detail = new ApprenticeshipApplicationDetail
                {
                    LegacyApplicationId = oldId
                };

                var summary = new ApplicationStatusSummary
                {
                    ApplicationId = Guid.Empty,
                    LegacyApplicationId = newId
                };

                // Act.
                var actual = detail.UpdateApprenticeshipApplicationDetail(summary, _repository.Object);

                // Assert.
                actual.Should().Be(expected);
                detail.LegacyApplicationId.Should().Be(summary.LegacyApplicationId);
            }

            [TestCase("A", "B", true)]
            [TestCase(null, "A", true)]
            [TestCase("A", null, true)]
            [TestCase("A", "A", false)]
            [TestCase(null, null, false)]
            public void ShouldUpdateUnsuccessfulReason(string oldReason, string newReason, bool expected)
            {
                // Arrange.
                var detail = new ApprenticeshipApplicationDetail
                {
                    UnsuccessfulReason = oldReason
                };

                var summary = new ApplicationStatusSummary
                {
                    ApplicationId = Guid.Empty,
                    UnsuccessfulReason = newReason
                };

                // Act.
                var actual = detail.UpdateApprenticeshipApplicationDetail(summary, _repository.Object);

                // Assert.
                actual.Should().Be(expected);
                detail.UnsuccessfulReason.Should().Be(summary.UnsuccessfulReason);
            }
        }

        public class WhenAnyUpdate
        {
            private Mock<IApprenticeshipApplicationWriteRepository> _repository;

            [SetUp]
            public void SetUp()
            {
                _repository = new Mock<IApprenticeshipApplicationWriteRepository>();
            }

            [TestCase(VacancyStatuses.Live, VacancyStatuses.Expired, true)]
            [TestCase(VacancyStatuses.Live, VacancyStatuses.Live, false)]
            public void ShouldUpdateVacancyStatus(
                VacancyStatuses oldStatus,
                VacancyStatuses newStatus,
                bool expected)
            {
                // Arrange.
                var detail = new ApprenticeshipApplicationDetail
                {
                    VacancyStatus = oldStatus
                };

                var summary = new ApplicationStatusSummary
                {
                    VacancyStatus = newStatus
                };

                // Act.
                var actual = detail.UpdateApprenticeshipApplicationDetail(summary, _repository.Object);

                // Assert.
                actual.Should().Be(expected);
                detail.VacancyStatus.Should().Be(summary.VacancyStatus);
            }

            [TestCase(0, 0, false)]
            [TestCase(0, 1, true)]
            public void ShouldUpdateClosingDate(
                int oldClosingDateOffset,
                int newClosingDateOffset,
                bool expected)
            {
                // Arrange.
                var today = DateTime.Today;

                var detail = new ApprenticeshipApplicationDetail
                {
                    Vacancy = new ApprenticeshipSummary
                    {
                        ClosingDate = today.AddDays(oldClosingDateOffset)
                    }
                };

                var summary = new ApplicationStatusSummary
                {
                    ClosingDate = today.AddDays(newClosingDateOffset)
                };

                // Act.
                var actual = detail.UpdateApprenticeshipApplicationDetail(summary, _repository.Object);

                // Assert.
                actual.Should().Be(expected);
                detail.Vacancy.ClosingDate.Should().Be(summary.ClosingDate);
            }
        }
    }
}
