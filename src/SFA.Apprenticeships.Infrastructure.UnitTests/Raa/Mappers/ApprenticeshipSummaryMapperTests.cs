namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa.Mappers
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Infrastructure.Raa.Mappers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class ApprenticeshipSummaryMapperTests
    {
        private Mock<ILogService> _mockLogService;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
        }

        [TestCase(10, 5)]
        public void ShouldGetApprenticeshipSummary(int vacancyCount, int categoryCount)
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
                var summary = ApprenticeshipSummaryMapper.GetApprenticeshipSummary(
                    vacancy, employer, provider, categories, _mockLogService.Object);

                // Assert.
                summary.Should().NotBeNull();
            }
        }

        [TestCase(10)]
        public void ShouldLogAndReturnNullOnFailureToGetApprenticeshipSummary(int categoryCount)
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
            var summary = ApprenticeshipSummaryMapper.GetApprenticeshipSummary(
                null, employer, provider, categories, _mockLogService.Object);

            // Assert.
            summary.Should().BeNull();
            _mockLogService.Verify(mock =>
                mock.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [TestCase("Acme Corp", "Anonymous Employer Name", true)]
        [TestCase("Evil Inc", null, false)]
        [TestCase("Awesome Ltd", "", false)]
        public void ShouldHandleEmployerNameAnonymisation(
            string employerName, string anonymousEmployerName, bool anonymised)
        {
            // Arrange.
            var fixture = new Fixture();

            var vacancy = fixture.Create<Domain.Entities.Raa.Vacancies.VacancySummary>();
            var provider = fixture.Create<Domain.Entities.Raa.Parties.Provider>();

            var employer = fixture
                .Build<Domain.Entities.Raa.Parties.Employer>()
                .With(each => each.Name, employerName)
                .With(each => each.TradingName, anonymousEmployerName)
                .Create();

            var categories = fixture
                .Build<Domain.Entities.ReferenceData.Category>()
                .CreateMany(1)
                .ToList();

            // Act.
            var summary = ApprenticeshipSummaryMapper.GetApprenticeshipSummary(
                vacancy, employer, provider, categories, _mockLogService.Object);

            // Assert.
            summary.Should().NotBeNull();
            summary.EmployerName.Should().Be(anonymised ? anonymousEmployerName : employerName);
        }
    }
}
