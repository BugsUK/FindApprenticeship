namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Candidates
{
    using System.Collections.Generic;
    using Application.Interfaces.Vacancies;
    using Builder;
    using Entities.Candidates;
    using Entities.Vacancies;
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

        [TestCase(ApprenticeshipSearchField.All, "")]
        [TestCase(ApprenticeshipSearchField.JobTitle, "Job title: ")]
        [TestCase(ApprenticeshipSearchField.Description, "Description: ")]
        [TestCase(ApprenticeshipSearchField.Employer, "Employer: ")]
        [TestCase(ApprenticeshipSearchField.ReferenceNumber, "")]
        public void KeywordSearchFieldSearchName(ApprenticeshipSearchField apprenticeshipSearchField, string expectedPrefix)
        {
            var savedSearch = new SavedSearchBuilder().WithSearchField(apprenticeshipSearchField.ToString()).WithKeywords("engineering").WithinDistance(5).WithLocation("CV1 2WT").Build();

            var name = savedSearch.Name();

            name.Should().Be(string.Format("{0}engineering within 5 miles of CV1 2WT", expectedPrefix));
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
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Keyword&Location=Coventry&Hash=0&WithinDistance=10&ApprenticeshipLevel=All&SearchField=All&DisplaySubCategory=True&DisplayDescription=True&DisplayDistance=True&DisplayClosingDate=True&DisplayStartDate=True&DisplayApprenticeshipLevel=False&DisplayWage=False&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void LocationLatLongSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithinDistance(10).WithLocation("Coventry").WithLatLong(1.1d, 2.1d).Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().BeOneOf(
                "/apprenticeships?SearchMode=Keyword&Location=Coventry&Longitude=2.1&Latitude=1.1&Hash=-1434161545&WithinDistance=10&ApprenticeshipLevel=All&SearchField=All&DisplaySubCategory=True&DisplayDescription=True&DisplayDistance=True&DisplayClosingDate=True&DisplayStartDate=True&DisplayApprenticeshipLevel=False&DisplayWage=False&SearchAction=Search&LocationType=NonNational",
                "/apprenticeships?SearchMode=Keyword&Location=Coventry&Longitude=2.1&Latitude=1.1&Hash=-94534883&WithinDistance=10&ApprenticeshipLevel=All&SearchField=All&DisplaySubCategory=True&DisplayDescription=True&DisplayDistance=True&DisplayClosingDate=True&DisplayStartDate=True&DisplayApprenticeshipLevel=False&DisplayWage=False&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void KeywordSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithKeywords("engineering").WithinDistance(5).WithLocation("CV1 2WT").Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Keyword&Location=CV1+2WT&Hash=0&Keywords=engineering&WithinDistance=5&ApprenticeshipLevel=All&SearchField=All&DisplaySubCategory=True&DisplayDescription=True&DisplayDistance=True&DisplayClosingDate=True&DisplayStartDate=True&DisplayApprenticeshipLevel=False&DisplayWage=False&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void EnglandSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithKeywords("engineering").WithinDistance(0).WithLocation("CV1 2WT").Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Keyword&Location=CV1+2WT&Hash=0&Keywords=engineering&WithinDistance=0&ApprenticeshipLevel=All&SearchField=All&DisplaySubCategory=True&DisplayDescription=True&DisplayDistance=True&DisplayClosingDate=True&DisplayStartDate=True&DisplayApprenticeshipLevel=False&DisplayWage=False&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void SectorSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithSearchMode(ApprenticeshipSearchMode.Category).WithCategory("CST").WithinDistance(0).Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Category&Hash=0&WithinDistance=0&ApprenticeshipLevel=All&Category=CST&SearchField=All&DisplaySubCategory=True&DisplayDescription=True&DisplayDistance=True&DisplayClosingDate=True&DisplayStartDate=True&DisplayApprenticeshipLevel=False&DisplayWage=False&SearchAction=Search&LocationType=NonNational");
        }

        [Test]
        public void SectorFrameworkSearchUrl()
        {
            var savedSearch = new SavedSearchBuilder().WithSearchMode(ApprenticeshipSearchMode.Category).WithLocation("CV1 2WT").WithCategory("CST").WithSubCategories(new[] { "522", "532" }).WithinDistance(10).Build();

            var searchUrl = savedSearch.SearchUrl();

            searchUrl.Should().NotBeNull();
            searchUrl.Value.Should().Be("/apprenticeships?SearchMode=Category&Location=CV1+2WT&Hash=0&WithinDistance=10&ApprenticeshipLevel=All&Category=CST&SearchField=All&DisplaySubCategory=True&DisplayDescription=True&DisplayDistance=True&DisplayClosingDate=True&DisplayStartDate=True&DisplayApprenticeshipLevel=False&DisplayWage=False&SearchAction=Search&LocationType=NonNational&SubCategories=522&SubCategories=532");
        }

        [Test]
        public void TruncatedSubCategoriesFullNames_One()
        {
            var savedSearch = new SavedSearchBuilder().WithSubCategoriesFullNames(GetSubCategoriesFullNames(1, ", ")).Build();

            var truncatedSubCategoriesFullNames = savedSearch.TruncatedSubCategoriesFullNames(5);

            truncatedSubCategoriesFullNames.Should().NotBeNull();
            truncatedSubCategoriesFullNames.Should().Be("Sub Category 01");
        }

        [Test]
        public void TruncatedSubCategoriesFullNames_Five()
        {
            var savedSearch = new SavedSearchBuilder().WithSubCategoriesFullNames(GetSubCategoriesFullNames(5, "|")).Build();

            var truncatedSubCategoriesFullNames = savedSearch.TruncatedSubCategoriesFullNames(5);

            truncatedSubCategoriesFullNames.Should().NotBeNull();
            truncatedSubCategoriesFullNames.Should().Be("Sub Category 01, Sub Category 02, Sub Category 03, Sub Category 04, Sub Category 05");
        }

        [Test]
        public void TruncatedSubCategoriesFullNames_Six()
        {
            var savedSearch = new SavedSearchBuilder().WithSubCategoriesFullNames(GetSubCategoriesFullNames(6, ", ")).Build();

            var truncatedSubCategoriesFullNames = savedSearch.TruncatedSubCategoriesFullNames(5);

            truncatedSubCategoriesFullNames.Should().NotBeNull();
            truncatedSubCategoriesFullNames.Should().Be("Sub Category 01, Sub Category 02, Sub Category 03, Sub Category 04, Sub Category 05 and 1 more");
        }

        [Test]
        public void TruncatedSubCategoriesFullNames_Seven()
        {
            var savedSearch = new SavedSearchBuilder().WithSubCategoriesFullNames(GetSubCategoriesFullNames(7, "|")).Build();

            var truncatedSubCategoriesFullNames = savedSearch.TruncatedSubCategoriesFullNames(5);

            truncatedSubCategoriesFullNames.Should().NotBeNull();
            truncatedSubCategoriesFullNames.Should().Be("Sub Category 01, Sub Category 02, Sub Category 03, Sub Category 04, Sub Category 05 and 2 more");
        }

        private static string GetSubCategoriesFullNames(int count, string separator)
        {
            var subCategoriesFullNames = new List<string>();

            for (var i = 0; i < count; i++)
            {
                subCategoriesFullNames.Add(string.Format("Sub Category {0:D2}", i + 1));
            }

            return string.Join(separator, subCategoriesFullNames);
        }
    }
}