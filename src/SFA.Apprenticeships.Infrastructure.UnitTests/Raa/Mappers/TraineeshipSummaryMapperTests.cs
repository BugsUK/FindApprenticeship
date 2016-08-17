namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa.Mappers
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Infrastructure.Raa.Mappers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    [Parallelizable]
    public class TraineeshipSummaryMapperTests
    {
        private Mock<ILogService> _mockLogService;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
        }

        [TestCase(10, 5)]
        public void ShouldGetTraineeshipSummary(int vacancyCount, int categoryCount)
        {
            for (var i = 0; i < vacancyCount; i++)
            {
                // Arrange.
                var fixture = new Fixture();

                var vacancy = fixture.Create<Domain.Entities.Raa.Vacancies.VacancySummary>();
                var employer = fixture.Create<Domain.Entities.Raa.Parties.Employer>();
                var provider = fixture.Create<Domain.Entities.Raa.Parties.Provider>();

                var categories = fixture
                    .Build<Domain.Entities.ReferenceData.Category>()
                    .CreateMany(categoryCount)
                    .ToList();

                // Act.
                var summary = TraineeshipSummaryMapper.GetTraineeshipSummary(
                    vacancy, employer, provider, categories, _mockLogService.Object);

                // Assert.
                summary.Should().NotBeNull();
            }
        }

        [TestCase(10)]
        public void ShouldLogAndReturnNullOnFailureToGetTraineeshipSummary(int categoryCount)
        {
            // Arrange.
            var fixture = new Fixture();

            var employer = fixture.Create<Domain.Entities.Raa.Parties.Employer>();
            var provider = fixture.Create<Domain.Entities.Raa.Parties.Provider>();

            var categories = fixture
                .Build<Domain.Entities.ReferenceData.Category>()
                .CreateMany(categoryCount)
                .ToList();

            // Act.
            var summary = TraineeshipSummaryMapper.GetTraineeshipSummary(
                null, employer, provider, categories, _mockLogService.Object);

            // Assert.
            summary.Should().BeNull();
            _mockLogService.Verify(mock =>
                mock.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [TestCase("Anonymous Employer Name", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        public void ShouldHandleEmployerNameAnonymisation(
            string anonymousEmployerName, bool anonymised)
        {
            // Arrange.
            var fixture = new Fixture();

            var vacancy = fixture
                .Build<Domain.Entities.Raa.Vacancies.VacancySummary>()
                .With(each => each.EmployerAnonymousName, anonymousEmployerName)
                .Create();

            var provider = fixture.Create<Domain.Entities.Raa.Parties.Provider>();
            var employer = fixture.Create<Domain.Entities.Raa.Parties.Employer>();

            var categories = fixture
                .Build<Domain.Entities.ReferenceData.Category>()
                .CreateMany(1)
                .ToList();

            // Act.
            var summary = TraineeshipSummaryMapper.GetTraineeshipSummary(
                vacancy, employer, provider, categories, _mockLogService.Object);

            // Assert.
            summary.Should().NotBeNull();
            summary.EmployerName.Should().Be(anonymised ? string.Empty : employer.Name);
        }
    }
}
