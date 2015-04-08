namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candidate.Mediators.Search;
    using Constants;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests : TestsBase
    {
        [TestCase(ApprenticeshipSearchMode.Keyword)]
        [TestCase(ApprenticeshipSearchMode.Category)]
        [TestCase(ApprenticeshipSearchMode.SavedSearches)]
        public void Ok_AllApprenticeshipSearchModes(ApprenticeshipSearchMode searchMode)
        {
            var candidateId = searchMode == ApprenticeshipSearchMode.SavedSearches
                ? Guid.NewGuid()
                : default(Guid?);

            var response = Mediator.Index(candidateId, searchMode);

            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(5);
            viewModel.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be("All");
            viewModel.SearchMode.Should().Be(searchMode);

            if (searchMode == ApprenticeshipSearchMode.SavedSearches)
            {
                viewModel.SavedSearches.Should().NotBeNull();
            }
            else
            {
                viewModel.SavedSearches.Should().BeNull();
            }
        }

        [Test]
        public void CandidateNotLoggedIn_SavedSearchesMode()
        {
            var response = Mediator.Index(null, ApprenticeshipSearchMode.SavedSearches);

            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;

            viewModel.WithinDistance.Should().Be(5);
            viewModel.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be("All");
            viewModel.SearchMode.Should().Be(ApprenticeshipSearchMode.Keyword);
            viewModel.SavedSearches.Should().BeNull();
        }

        [Test]
        public void BlacklistedCategoryCodes()
        {
            ReferenceDataService.Setup(rds => rds.GetCategories()).Returns(GetCategories);

            var response = Mediator.Index(null, ApprenticeshipSearchMode.Category);

            var categories = response.ViewModel.Categories;
            categories.Count.Should().Be(3);
            categories.Any(c => c.CodeName == "00").Should().BeFalse();
            categories.Any(c => c.CodeName == "99").Should().BeFalse();
        }

        [Test]
        public void RememberApprenticeshipLevel()
        {
            UserDataProvider.Setup(udp => udp.Get(CandidateDataItemNames.ApprenticeshipLevel)).Returns("Advanced");

            var response = Mediator.Index(null, ApprenticeshipSearchMode.Keyword);

            var viewModel = response.ViewModel;
            viewModel.ApprenticeshipLevel.Should().Be("Advanced");
        }

        [Test]
        public void ShowEnglandSearchInKeywordSearch()
        {
            var response = Mediator.Index(null, ApprenticeshipSearchMode.Keyword);

            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.Distances.Last().Text.Should().Be("England");
            viewModel.Distances.Last().Value.Should().Be("0");
        }

        [Test]
        public void ShowEnglandSearchInCategorySearch()
        {
            var response = Mediator.Index(null, ApprenticeshipSearchMode.Category);

            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.Distances.Last().Text.Should().Be("England");
            viewModel.Distances.Last().Value.Should().Be("0");
        }

        private static IEnumerable<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    CodeName = "1",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "1_1"},
                        new Category {CodeName = "1_2"}
                    }
                },
                new Category
                {
                    CodeName = "2",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "2_1"},
                        new Category {CodeName = "2_2"},
                        new Category {CodeName = "2_3"}
                    }
                },
                new Category
                {
                    CodeName = "3",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "3_1"}
                    }
                },
                new Category
                {
                    CodeName = "00",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "00_1"}
                    }
                },
                new Category
                {
                    CodeName = "99",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "99_1"}
                    }
                }
            };
        }
    }
}