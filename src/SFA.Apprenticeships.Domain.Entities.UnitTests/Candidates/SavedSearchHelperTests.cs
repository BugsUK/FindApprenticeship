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

            name.Should().Be("Within 10 miles of Coventry");
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
            var savedSearch = new SavedSearchBuilder().WithSearchMode(ApprenticeshipSearchMode.Category).WithCategory("CST").WithCategoryFullName("Construction, Planning and the Built Environment").WithinDistance(0).Build();

            var name = savedSearch.Name();

            name.Should().Be("Construction, Planning and the Built Environment within England");
        }

        [Test]
        public void SectorFrameworkName()
        {
            var savedSearch = new SavedSearchBuilder().WithSearchMode(ApprenticeshipSearchMode.Category).WithLocation("CV1 2WT").WithCategory("CST").WithCategoryFullName("Construction, Planning and the Built Environment").WithSubCategories(new[] { "522", "532" }).WithinDistance(10).Build();

            var name = savedSearch.Name();

            name.Should().Be("Construction, Planning and the Built Environment within 10 miles of CV1 2WT");
        }

        [Test]
        public void LocationSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithinDistance(10).WithLocation("Coventry").Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Keyword&Location=Coventry&Hash=0&WithinDistance=10&ApprenticeshipLevel=All&SearchField=All&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void LocationLatLongSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithinDistance(10).WithLocation("Coventry").WithLatLong(1.1d, 2.1d).Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Keyword&Location=Coventry&Longitude=2.1&Latitude=1.1&Hash=-1434161545&WithinDistance=10&ApprenticeshipLevel=All&SearchField=All&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void KeywordSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithKeywords("engineering").WithinDistance(5).WithLocation("CV1 2WT").Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Keyword&Location=CV1+2WT&Hash=0&Keywords=engineering&WithinDistance=5&ApprenticeshipLevel=All&SearchField=All&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void EnglandSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithKeywords("engineering").WithinDistance(0).WithLocation("CV1 2WT").Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Keyword&Location=CV1+2WT&Hash=0&Keywords=engineering&WithinDistance=0&ApprenticeshipLevel=All&SearchField=All&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void SectorSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithSearchMode(ApprenticeshipSearchMode.Category).WithCategory("CST").WithinDistance(0).Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Category&Hash=0&WithinDistance=0&ApprenticeshipLevel=All&Category=CST&SearchField=All&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void SectorFrameworkSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithSearchMode(ApprenticeshipSearchMode.Category).WithLocation("CV1 2WT").WithCategory("CST").WithSubCategories(new[] { "522", "532" }).WithinDistance(10).Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Category&Location=CV1+2WT&Hash=0&WithinDistance=10&ApprenticeshipLevel=All&Category=CST&SearchField=All&SearchAction=Search&LocationType=NonNational&SubCategories=522&SubCategories=532");
        }
    }
}