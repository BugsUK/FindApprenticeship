namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.Factories
{
    using Apprenticeships.Application.Vacancies.Factories;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Interfaces.Vacancies;
    using NUnit.Framework;

    [TestFixture]
    public class SearchParametersFactoryTests
    {
        [Test]
        public void DefaultsTest()
        {
            var savedSearch = new SavedSearchBuilder().Build();
            var parameters = SearchParametersFactory.Create(savedSearch);
            parameters.Should().NotBeNull();
            parameters.PageSize.Should().Be(5);
            parameters.PageNumber.Should().Be(1);
            parameters.SortType.Should().Be(VacancySearchSortType.RecentlyAdded);
            parameters.VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        [Test]
        public void KeywordMappingTest()
        {
            const ApprenticeshipSearchMode searchMode = ApprenticeshipSearchMode.Keyword;
            const string keywords = "chef";
            const string location = "Warwick";
            const int withinDistance = 15;
            const string apprenticeshipLevel = "Advanced";
            const string category = "MFP";
            var subCategories = new[] { "513", "540" };
            const string searchField = "JobTitle";

            var savedSearch = new SavedSearchBuilder()
                .WithSearchMode(searchMode)
                .WithKeywords(keywords)
                .WithLocation(location)
                .WithinDistance(withinDistance)
                .WithApprenticeshipLevel(apprenticeshipLevel)
                .WithCategory(category)
                .WithSubCategories(subCategories)
                .WithSearchField(searchField)
                .Build();
            
            var parameters = SearchParametersFactory.Create(savedSearch);
            parameters.Should().NotBeNull();
            parameters.Keywords.Should().Be(keywords);
            parameters.Location.Should().NotBeNull();
            parameters.Location.Name.Should().Be(location);
            parameters.SearchRadius.Should().Be(withinDistance);
            parameters.ApprenticeshipLevel.Should().Be(apprenticeshipLevel);
            parameters.CategoryCode.Should().BeNullOrEmpty();
            parameters.SubCategoryCodes.Should().BeNull();
            parameters.SearchField.Should().Be(ApprenticeshipSearchField.JobTitle);
        }

        [Test]
        public void CategoryMappingTest()
        {
            const ApprenticeshipSearchMode searchMode = ApprenticeshipSearchMode.Category;
            const string keywords = "chef";
            const string location = "Warwick";
            const int withinDistance = 15;
            const string apprenticeshipLevel = "Advanced";
            const string category = "MFP";
            var subCategories = new[] { "513", "540" };
            const string searchField = "JobTitle";

            var savedSearch = new SavedSearchBuilder()
                .WithSearchMode(searchMode)
                .WithKeywords(keywords)
                .WithLocation(location)
                .WithinDistance(withinDistance)
                .WithApprenticeshipLevel(apprenticeshipLevel)
                .WithCategory(category)
                .WithSubCategories(subCategories)
                .WithSearchField(searchField)
                .Build();

            var parameters = SearchParametersFactory.Create(savedSearch);
            parameters.Should().NotBeNull();
            parameters.Keywords.Should().BeNullOrEmpty();
            parameters.Location.Should().NotBeNull();
            parameters.Location.Name.Should().Be(location);
            parameters.SearchRadius.Should().Be(withinDistance);
            parameters.ApprenticeshipLevel.Should().Be(apprenticeshipLevel);
            parameters.CategoryCode.Should().Be(category);
            parameters.SubCategoryCodes.Should().BeEquivalentTo(subCategories);
            parameters.SearchField.Should().Be(ApprenticeshipSearchField.All);
        }

        [Test]
        public void LatLongTest()
        {
            var savedSearch = new SavedSearchBuilder().WithLatLong(1.1, 2.1).Build();
            var parameters = SearchParametersFactory.Create(savedSearch);
            parameters.Location.Should().NotBeNull();
            parameters.Location.GeoPoint.Should().NotBeNull();
            parameters.Location.GeoPoint.Latitude.Should().Be(1.1);
            parameters.Location.GeoPoint.Longitude.Should().Be(2.1);
        }
    }
}