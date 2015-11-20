namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers
{
    using AvService.Mappers.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ApprenticeshipVacancyQueryMapperTests
    {
        private ApprenticeshipVacancyQueryMapper _apprenticeshipVacancyQueryMapper;

        [SetUp]
        public void SetUp()
        {
            _apprenticeshipVacancyQueryMapper = new ApprenticeshipVacancyQueryMapper();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("ABC1")]
        public void ShouldMapFrameworkCode(string frameworkCode)
        {
            // Arrange.
            var criteria = new VacancySearchData
            {
                FrameworkCode = frameworkCode
            };

            // Act.
            var mappedQuery = _apprenticeshipVacancyQueryMapper.MapToApprenticeshipVacancyQuery(criteria);

            // Assert.
            mappedQuery.FrameworkCodeName.Should().Be(frameworkCode);
        }

        [TestCase(0, 1)]
        [TestCase(5, 5)]
        public void ShouldMapCurrentPage(int pageIndex, int expectedCurrentPage)
        {
            // Arrange.
            var criteria = new VacancySearchData
            {
                PageIndex = pageIndex
            };

            // Act.
            var mappedQuery = _apprenticeshipVacancyQueryMapper.MapToApprenticeshipVacancyQuery(criteria);

            // Assert.
            mappedQuery.CurrentPage.Should().Be(expectedCurrentPage);
        }

        [Test]
        public void ShouldDefaultPageSize()
        {
            // Arrange.
            var criteria = new VacancySearchData();

            // Act.
            var mappedQuery = _apprenticeshipVacancyQueryMapper.MapToApprenticeshipVacancyQuery(criteria);

            // Assert.
            mappedQuery.PageSize.Should().Be(ApprenticeshipVacancyQueryMapper.DefaultPageSize);
        }
    }
}
