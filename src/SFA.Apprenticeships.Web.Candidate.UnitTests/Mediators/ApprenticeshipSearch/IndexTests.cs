namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candidate.Mediators.Search;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
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

            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails {Address = new Address {Postcode = "CANDIDATE POSTCODE"}}
            };
            CandidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);

            var response = Mediator.Index(candidateId, searchMode, false);

            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(5);
            viewModel.LocationType.Should().Be(VacancyLocationType.NonNational);
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


        private static IEnumerable<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category(1, "1", "1", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                {
                    new Category(1, "1_1", "1_1", CategoryType.Framework, CategoryStatus.Active),
                    new Category(1, "1_2", "1_2", CategoryType.Framework, CategoryStatus.Active)
                }
                    ),
                new Category(2, "2", "2", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                {
                    new Category(1, "2_1", "2_1", CategoryType.Framework, CategoryStatus.Active),
                    new Category(2, "2_2", "2_2", CategoryType.Framework, CategoryStatus.Active),
                    new Category(3, "2_3", "2_3", CategoryType.Framework, CategoryStatus.Active)
                }
                    ),
                new Category(3, string.Empty, "3", "3", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                {
                    new Category(1, "3_1", "3_1", CategoryType.Framework, CategoryStatus.Active)
                }
                    ),
                new Category(0, "00", "00", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                {
                    new Category(1, "00_1", "00_1", CategoryType.Framework, CategoryStatus.Active)
                }
                    ),
                new Category(99, "99", "99", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                {
                    new Category(1, "99_1", "99_1", CategoryType.Framework, CategoryStatus.Active)
                }
                    )
            };
        }

        [Test]
        public void BlacklistedCategoryCodes()
        {
            ReferenceDataService.Setup(rds => rds.GetCategories()).Returns(GetCategories);

            var response = Mediator.Index(null, ApprenticeshipSearchMode.Category, false);

            var categories = response.ViewModel.Categories;
            categories.Count.Should().Be(3);
            categories.Any(c => c.CodeName == "00").Should().BeFalse();
            categories.Any(c => c.CodeName == "99").Should().BeFalse();
        }

        [Test]
        public void CandidateNotLoggedIn_SavedSearchesMode()
        {
            var response = Mediator.Index(null, ApprenticeshipSearchMode.SavedSearches, false);

            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;

            viewModel.WithinDistance.Should().Be(5);
            viewModel.LocationType.Should().Be(VacancyLocationType.NonNational);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be("All");
            viewModel.SearchMode.Should().Be(ApprenticeshipSearchMode.Keyword);
            viewModel.SavedSearches.Should().BeNull();
        }

        [Test]
        public void PoputlateLocationCookieWithSearchedLocation()
        {
            UserDataProvider.Setup(x => x.Get(UserDataItemNames.LastSearchedLocation)).Returns("TEST COOKIE LOCATION");

            var response = Mediator.Index(null, ApprenticeshipSearchMode.Keyword, false);
            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);
            response.ViewModel.Location.Should().Be("TEST COOKIE LOCATION");
            CandidateServiceProvider.Verify(x => x.GetCandidate(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public void PoputlateLocationWithUsersPostcode()
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails {Address = new Address {Postcode = "CANDIDATE POSTCODE"}}
            };

            CandidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);

            var response = Mediator.Index(Guid.NewGuid(), ApprenticeshipSearchMode.Keyword, true);
            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);
            response.ViewModel.Location.Should().Be("CANDIDATE POSTCODE");

            UserDataProvider.Verify(x => x.Push(UserDataItemNames.LastSearchedLocation, "CANDIDATE POSTCODE"),
                Times.Once);
            CandidateServiceProvider.Verify(x => x.GetCandidate(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void RememberApprenticeshipLevel()
        {
            UserDataProvider.Setup(udp => udp.Get(CandidateDataItemNames.ApprenticeshipLevel)).Returns("Advanced");

            var response = Mediator.Index(null, ApprenticeshipSearchMode.Keyword, false);

            var viewModel = response.ViewModel;
            viewModel.ApprenticeshipLevel.Should().Be("Advanced");
        }

        [Test]
        public void ShowEnglandSearchInCategorySearch()
        {
            var response = Mediator.Index(null, ApprenticeshipSearchMode.Category, false);

            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.Distances.Last().Text.Should().Be("England");
            viewModel.Distances.Last().Value.Should().Be("0");
        }

        [Test]
        public void ShowEnglandSearchInKeywordSearch()
        {
            var response = Mediator.Index(null, ApprenticeshipSearchMode.Keyword, false);

            response.AssertCode(ApprenticeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.Distances.Last().Text.Should().Be("England");
            viewModel.Distances.Last().Value.Should().Be("0");
        }
    }
}