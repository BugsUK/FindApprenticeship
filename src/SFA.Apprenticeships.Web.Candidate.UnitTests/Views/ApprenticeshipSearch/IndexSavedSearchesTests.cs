namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candidate.ViewModels.Account;
    using Candidate.Views.ApprenticeshipSearch;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;

    [TestFixture]
    public class IndexSavedSearchesTests : MediatorTestsBase
    {
        [Test]
        public void ShouldRenderTabAsInactiveWhenSearchModeIsKeyword()
        {
            // Arrange.
            var @partial = new savedSearches();
            var viewModel = Mediator.Index(null, ApprenticeshipSearchMode.Keyword, false).ViewModel;

            // Act.
            var view = @partial.RenderAsHtml(viewModel);

            // Assert.
            view.GetElementbyId("saved-searches").Attributes["class"].Value.Contains(" active").Should().BeFalse();
        }

        [Test]
        public void ShouldRenderTabAsInactiveWhenSearchModeIsCategory()
        {
            // Arrange.
            var @partial = new savedSearches();
            var viewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category, false).ViewModel;

            // Act.
            var view = @partial.RenderAsHtml(viewModel);

            // Assert.
            view.GetElementbyId("saved-searches").Attributes["class"].Value.Contains(" active").Should().BeFalse();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldRenderTabAsActiveWhenSearchModeIsSavedSearches(int savedSearchCount)
        {
            // Arrange.
            var @partial = new savedSearches();
            var candidateId = Guid.NewGuid();
            var mockViewModel = new Fixture().Build<SavedSearchViewModel>().CreateMany(savedSearchCount);

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            CandidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var viewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;

            // Act.
            var view = @partial.RenderAsHtml(viewModel);

            // Assert.
            view.GetElementbyId("saved-searches").Attributes["class"].Value.Contains(" active").Should().BeTrue();
        }

        [Test]
        public void ShouldRenderSavedSearchPromptWhenCandidateHasNoSavedSearches()
        {
            // Arrange.
            var @partial = new savedSearches();
            var candidateId = Guid.NewGuid();

            var mockViewModel = new List<SavedSearchViewModel>();

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            CandidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var indexViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;

            // Act.
            var view = @partial.RenderAsHtml(indexViewModel);

            // Assert.
            var list = view.GetElementbyId("saved-searches-list");
            var prompt = view.GetElementbyId("saved-searches-prompt");

            list.Should().BeNull();
            prompt.Should().NotBeNull();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldRenderSavedSearchesList(int savedSearchCount)
        {
            // Arrange.
            var @partial = new savedSearches();
            var candidateId = Guid.NewGuid();

            var mockViewModel = new Fixture().Build<SavedSearchViewModel>().CreateMany(savedSearchCount).ToList();

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            CandidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var indexViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;

            // Act.
            var view = @partial.RenderAsHtml(indexViewModel);

            // Assert.
            var list = view.GetElementbyId("saved-searches-list");

            list.Should().NotBeNull();
            list.ChildNodes.Count(node => node.Name == "li").Should().Be(savedSearchCount);

            foreach (var savedSearchViewModel in mockViewModel)
            {
                var listItemId = string.Format("{0}-{1}", "saved-search", savedSearchViewModel.Id);
                var listItemLabelId = string.Format("{0}-{1}", "saved-search-label", savedSearchViewModel.Id);

                var listItem = view.GetElementbyId(listItemId);

                listItem.Should().NotBeNull();
                listItem.GetAttributeValue("value", null).Should().Be(savedSearchViewModel.Id.ToString());

                var listItemLabel = view.GetElementbyId(listItemLabelId);

                listItemLabel.Should().NotBeNull();
                listItemLabel.InnerText.Should().Be(savedSearchViewModel.Name);
            }
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldRenderSubcategoriesFilter(int savedSearchCount)
        {
            // Arrange.
            var @partial = new savedSearches();
            var candidateId = Guid.NewGuid();

            var mockViewModel = new Fixture().Build<SavedSearchViewModel>().CreateMany(savedSearchCount).ToList();

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            CandidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var indexViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;

            // Act.
            var view = @partial.RenderAsHtml(indexViewModel);

            // Assert.
            var list = view.GetElementbyId("saved-searches-list");

            list.Should().NotBeNull();
            list.ChildNodes.Count(node => node.Name == "li").Should().Be(savedSearchCount);

            foreach (var savedSearchViewModel in mockViewModel)
            {
                var id = string.Format("{0}-{1}", "saved-search-subcategories", savedSearchViewModel.Id);

                var element = view.GetElementbyId(id);

                element.Should().NotBeNull();
                element.InnerText.Should().Be(savedSearchViewModel.SubCategoriesFullNames);
            }
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldNotRenderSubcategoriesFilter(int savedSearchCount)
        {
            // Arrange.
            var @partial = new savedSearches();
            var candidateId = Guid.NewGuid();

            var mockViewModel = new Fixture()
                .Build<SavedSearchViewModel>()
                .With(each => each.SubCategoriesFullNames, null)
                .CreateMany(savedSearchCount).ToList();

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            CandidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var indexViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;

            // Act.
            var view = @partial.RenderAsHtml(indexViewModel);

            // Assert.
            var list = view.GetElementbyId("saved-searches-list");

            list.Should().NotBeNull();
            list.ChildNodes.Count(node => node.Name == "li").Should().Be(savedSearchCount);

            foreach (var savedSearchViewModel in mockViewModel)
            {
                var id = string.Format("{0}-{1}", "saved-search-subcategories", savedSearchViewModel.Id);

                var element = view.GetElementbyId(id);

                element.Should().BeNull();
            }
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldRenderApprenticeshipLevelFilterWhenNotAll(int savedSearchCount)
        {
            // Arrange.
            var @partial = new savedSearches();
            var candidateId = Guid.NewGuid();

            var mockViewModel = new Fixture().Build<SavedSearchViewModel>().CreateMany(savedSearchCount).ToList();

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            CandidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var indexViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;

            // Act.
            var view = @partial.RenderAsHtml(indexViewModel);

            // Assert.
            var list = view.GetElementbyId("saved-searches-list");

            list.Should().NotBeNull();
            list.ChildNodes.Count(node => node.Name == "li").Should().Be(savedSearchCount);

            foreach (var savedSearchViewModel in mockViewModel)
            {
                var id = string.Format("{0}-{1}", "saved-search-apprenticeship-level", savedSearchViewModel.Id);

                var element = view.GetElementbyId(id);

                element.Should().NotBeNull();
                element.InnerText.Should().Be(savedSearchViewModel.ApprenticeshipLevel);
            }
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldNotRenderApprenticeshipLevelFilterAll(int savedSearchCount)
        {
            // Arrange.
            var @partial = new savedSearches();
            var candidateId = Guid.NewGuid();

            var mockViewModel = new Fixture()
                .Build<SavedSearchViewModel>()
                .With(each => each.ApprenticeshipLevel, "All")
                .CreateMany(savedSearchCount).ToList();

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            CandidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var indexViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;

            // Act.
            var view = @partial.RenderAsHtml(indexViewModel);

            // Assert.
            var list = view.GetElementbyId("saved-searches-list");

            list.Should().NotBeNull();
            list.ChildNodes.Count(node => node.Name == "li").Should().Be(savedSearchCount);

            foreach (var savedSearchViewModel in mockViewModel)
            {
                var id = string.Format("{0}-{1}", "saved-search-apprenticeship-level", savedSearchViewModel.Id);

                view.GetElementbyId(id).Should().BeNull();
            }
        }
    }
}