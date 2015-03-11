namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Candidates
{
    using Builder;
    using Entities.Candidates;
    using Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SavedSearchHelperTests
    {
        [Test]
        public void LocationSearchName()
        {
            var savedSearch = new SavedSearchBuilder().WithinDistance(10).WithLocation("Coventry").Build();

            var name = savedSearch.Name();

            name.Should().Be("All within 10 miles of Coventry");
        }

        [Test]
        public void KeywordSearchName()
        {
            var savedSearch = new SavedSearchBuilder().WithKeywords("engineering").WithinDistance(5).WithLocation("CV1 2WT").Build();

            var name = savedSearch.Name();

            name.Should().Be("engineering within 5 miles of CV1 2WT");
        }

        [Test]
        public void EnglandSearchName()
        {
            var savedSearch = new SavedSearchBuilder().WithKeywords("engineering").WithinDistance(0).WithLocation("CV1 2WT").Build();

            var name = savedSearch.Name();

            name.Should().Be("engineering within England");
        }

        [Test]
        public void SectorSearchName()
        {
            var savedSearch = new SavedSearchBuilder().WithSearchMode(ApprenticeshipSearchMode.Category).WithCategory("Construction, Planning and the Built Environment").WithinDistance(0).Build();

            var name = savedSearch.Name();

            name.Should().Be("Construction, Planning and the Built Environment within England");
        }
    }
}