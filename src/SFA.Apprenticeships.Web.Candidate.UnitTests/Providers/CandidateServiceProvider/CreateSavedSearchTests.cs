namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Candidates;
    using Builders;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CreateSavedSearchTests
    {
        [Test]
        public void Success()
        {
            var candidateId = Guid.NewGuid();
            SavedSearch savedSearch = null;
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>())).Callback<SavedSearch>(ss => { savedSearch = ss; });
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new ApprenticeshipSearchViewModelBuilder().Build();

            var response = provider.CreateSavedSearch(candidateId, viewModel);

            response.Should().NotBeNull();
            candidateService.Verify(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>()), Times.Once);
            savedSearch.Should().NotBeNull();
        }

        [Test]
        public void AlertsEnabled()
        {
            var candidateId = Guid.NewGuid();
            SavedSearch savedSearch = null;
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>())).Callback<SavedSearch>(ss => { savedSearch = ss; });
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new ApprenticeshipSearchViewModelBuilder().Build();

            provider.CreateSavedSearch(candidateId, viewModel);

            savedSearch.AlertsEnabled.Should().BeTrue();
        }

        [Test]
        public void Mapping()
        {
            var candidateId = Guid.NewGuid();
            const ApprenticeshipSearchMode searchMode = ApprenticeshipSearchMode.Category;
            const string keywords = "chef";
            const string location = "Warwick";
            const double latitude = 1.1;
            const double longitude = 2.1;
            const int withinDistance = 15;
            const string apprenticeshipLevel = "Advanced";
            const string category = "MFP";
            const string categoryFullName = "Engineering and Manufacturing Technologies";
            var subCategories = new[] { "513", "540" };
            const string subCategoriesFullNames = "Surveying|Construction Civil Engineering";
            const string searchField = "JobTitle";
            const bool displaySubCategory = false;
            const bool displayDescription = false;
            const bool displayDistance = false;
            const bool displayClosingDate = false;
            const bool displayStartDate = false;
            const bool displayApprenticeshipLevel = true;
            const bool displayWage = true;

            var categories = new List<Category>
            {
                new Category(category, categoryFullName, CategoryType.SectorSubjectAreaTier1, new List<Category>
                    {
                        new Category("513", "Surveying", CategoryType.Framework),
                        new Category("540", "Construction Civil Engineering", CategoryType.Framework)
                    }
                )
            };

            SavedSearch savedSearch = null;
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetSavedSearches(candidateId)).Returns(new List<SavedSearch> { new SavedSearchBuilder().WithLocation("Different Location").Build() });
            candidateService.Setup(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>())).Callback<SavedSearch>(ss => { savedSearch = ss; });
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new ApprenticeshipSearchViewModelBuilder()
                .WithSearchMode(searchMode)
                .WithKeywords(keywords)
                .WithLocation(location)
                .WithLatLong(latitude, longitude)
                .WithinDistance(withinDistance)
                .WithApprenticeshipLevel(apprenticeshipLevel)
                .WithCategory(category)
                .WithSubCategories(subCategories)
                .WithSearchField(searchField)
                .WithCategories(categories)
                .WithDisplay(displaySubCategory, displayDescription, displayDistance, displayClosingDate, displayStartDate, displayApprenticeshipLevel, displayWage)
                .Build();

            var response = provider.CreateSavedSearch(candidateId, viewModel);

            response.Should().NotBeNull();
            candidateService.Verify(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>()), Times.Once);
            savedSearch.Should().NotBeNull();

            savedSearch.CandidateId.Should().Be(candidateId);
            savedSearch.SearchMode.Should().Be(searchMode);
            savedSearch.Keywords.Should().Be(keywords);
            savedSearch.Location.Should().Be(location);
            savedSearch.Latitude.Should().Be(latitude);
            savedSearch.Longitude.Should().Be(longitude);
            savedSearch.Hash.Should().Be(string.Format("{0}{1}{2}", longitude, latitude, location).GetHashCode());
            savedSearch.WithinDistance.Should().Be(withinDistance);
            savedSearch.ApprenticeshipLevel.Should().Be(apprenticeshipLevel);
            savedSearch.Category.Should().Be(category);
            savedSearch.CategoryFullName.Should().Be(categoryFullName);
            savedSearch.SubCategories.Should().BeEquivalentTo(subCategories);
            savedSearch.SubCategoriesFullName.Should().Be(subCategoriesFullNames);
            savedSearch.SearchField.Should().Be(searchField);
            savedSearch.LastResultsHash.Should().BeNull();
            savedSearch.DateProcessed.Should().Be(null);
            savedSearch.DisplaySubCategory.Should().Be(displaySubCategory);
            savedSearch.DisplayDescription.Should().Be(displayDescription);
            savedSearch.DisplayDistance.Should().Be(displayDistance);
            savedSearch.DisplayClosingDate.Should().Be(displayClosingDate);
            savedSearch.DisplayStartDate.Should().Be(displayStartDate);
            savedSearch.DisplayApprenticeshipLevel.Should().Be(displayApprenticeshipLevel);
            savedSearch.DisplayWage.Should().Be(displayWage);
        }

        [Test]
        public void SubCategoriesMustBelongToCategory()
        {
            var candidateId = Guid.NewGuid();
            const string category = "MFP";
            var subCategories = new[] { "513", "540", "600" };
            const string subCategoriesFullNames = "Surveying|Construction Civil Engineering";

            var categories = new List<Category>
            {
                new Category(category, category, CategoryType.SectorSubjectAreaTier1, new List<Category>
                    {
                        new Category("513", "Surveying", CategoryType.Framework),
                        new Category("540", "Construction Civil Engineering", CategoryType.Framework)
                    }
                ),
                new Category("OTHER", "OTHER", CategoryType.SectorSubjectAreaTier1, new List<Category>
                    {
                        new Category("600", "Should not be included", CategoryType.Framework)
                    }
                )
            };

            SavedSearch savedSearch = null;
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>())).Callback<SavedSearch>(ss => { savedSearch = ss; });
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new ApprenticeshipSearchViewModelBuilder()
                .WithCategory(category)
                .WithSubCategories(subCategories)
                .WithCategories(categories)
                .Build();

            var response = provider.CreateSavedSearch(candidateId, viewModel);

            response.Should().NotBeNull();
            candidateService.Verify(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>()), Times.Once);
            savedSearch.Should().NotBeNull();

            savedSearch.CandidateId.Should().Be(candidateId);
            savedSearch.Category.Should().Be(category);
            savedSearch.SubCategories.Length.Should().Be(2);
            savedSearch.SubCategories.Should().NotContain("600");
            savedSearch.SubCategoriesFullName.Should().Be(subCategoriesFullNames);
        }

        [Test]
        public void Error()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>())).Throws<Exception>();
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new ApprenticeshipSearchViewModelBuilder().Build();

            var response = provider.CreateSavedSearch(candidateId, viewModel);

            response.Should().NotBeNull();
            candidateService.Verify(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>()), Times.Once);
            response.HasError().Should().BeTrue();
            response.ViewModelMessage.Should().Be(VacancySearchResultsPageMessages.SaveSearchFailed);
        }

        [Test]
        public void CommunicationPreferences()
        {
            var candidateId = Guid.NewGuid();
            Candidate candidate = null;
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).EnableSavedSearchAlertsViaEmailAndText(false).Build());
            candidateService.Setup(cs => cs.SaveCandidate(It.IsAny<Candidate>())).Callback<Candidate>(c => { candidate = c; });
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new ApprenticeshipSearchViewModelBuilder().Build();

            provider.CreateSavedSearch(candidateId, viewModel);

            candidateService.Verify(cs => cs.GetCandidate(candidateId), Times.Once);
            candidateService.Verify(cs => cs.SaveCandidate(candidate), Times.Once);
        
            candidate.Should().NotBeNull();
            candidate.CommunicationPreferences.SavedSearchPreferences.EnableEmail.Should().BeTrue();
            candidate.CommunicationPreferences.SavedSearchPreferences.EnableText.Should().BeFalse();
        }

        [Test]
        public void DuplicatesAreReplaced()
        {
            var candidateId = Guid.NewGuid();
            const ApprenticeshipSearchMode searchMode = ApprenticeshipSearchMode.Category;
            const string keywords = "chef";
            const string location = "Warwick";
            const double latitude = 1.1;
            const double longitude = 2.1;
            const int withinDistance = 15;
            const string apprenticeshipLevel = "Advanced";
            const string category = "MFP";
            const string categoryFullName = "Engineering and Manufacturing Technologies";
            var subCategories = new[] { "513", "540" };
            const string subCategoriesFullNames = "Surveying, Construction Civil Engineering";
            const string searchField = "JobTitle";

            var categories = new List<Category>
            {
                new Category(category, categoryFullName, CategoryType.SectorSubjectAreaTier1, new List<Category>
                    {
                        new Category("513", "Surveying", CategoryType.Framework),
                        new Category("540", "Construction Civil Engineering", CategoryType.Framework)
                    }
                )
            };

            var existingSavedSearch = new SavedSearchBuilder()
                .WithSearchMode(searchMode)
                .WithKeywords(keywords)
                .WithLocation(location)
                .WithLatLong(latitude, longitude)
                .WithinDistance(withinDistance)
                .WithApprenticeshipLevel(apprenticeshipLevel)
                .WithCategory(category)
                .WithCategoryFullName(categoryFullName)
                .WithSubCategories(subCategories)
                .WithSubCategoriesFullNames(subCategoriesFullNames)
                .WithSearchField(searchField)
                .Build();

            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetSavedSearches(candidateId)).Returns(new List<SavedSearch> {existingSavedSearch});
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new ApprenticeshipSearchViewModelBuilder()
                .WithSearchMode(searchMode)
                .WithKeywords(keywords)
                .WithLocation(location)
                .WithLatLong(latitude, longitude)
                .WithinDistance(withinDistance)
                .WithApprenticeshipLevel(apprenticeshipLevel)
                .WithCategory(category)
                .WithSubCategories(subCategories)
                .WithSearchField(searchField)
                .WithCategories(categories)
                .Build();

            var response = provider.CreateSavedSearch(candidateId, viewModel);

            response.Should().NotBeNull();
            candidateService.Verify(cs => cs.GetSavedSearches(candidateId), Times.Once);

            //Should only replace search if it is new
            candidateService.Verify(cs => cs.CreateSavedSearch(It.IsAny<SavedSearch>()), Times.Never);
        }
    }
}