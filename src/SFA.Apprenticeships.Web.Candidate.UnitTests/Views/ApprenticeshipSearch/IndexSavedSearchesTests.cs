namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System;
    using System.Linq;
    using Candidate.ViewModels.Account;
    using Candidate.Views.ApprenticeshipSearch;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;

    public class IndexSavedSearchesTests : MediatorTestsBase
    {
        [Test]
        public void ShouldNotRenderTabAsActiveWhenSearchModeIsKeyword()
        {
            var @partial = new savedSearches();

            var viewModel = Mediator.Index(null, ApprenticeshipSearchMode.Keyword).ViewModel;
            var view = @partial.RenderAsHtml(viewModel);

            view.GetElementbyId("saved-searches").Attributes["class"].Value.Contains(" active").Should().BeFalse();
        }

        [Test]
        public void ShouldNotRenderTabAsActiveWhenSearchModeIsCategory()
        {
            var @partial = new savedSearches();

            var viewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category).ViewModel;
            var view = @partial.RenderAsHtml(viewModel);

            view.GetElementbyId("saved-searches").Attributes["class"].Value.Contains(" active").Should().BeFalse();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldRenderTabAsActiveWhenSearchModeIsSavedSearches(int savedSearchCount)
        {
            var @partial = new savedSearches();
            var candidateId = Guid.NewGuid();
            var mockViewModel = new Fixture().Build<SavedSearchViewModel>().CreateMany(savedSearchCount);

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var viewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches).ViewModel;
            var view = @partial.RenderAsHtml(viewModel);

            view.GetElementbyId("saved-searches").Attributes["class"].Value.Contains(" active").Should().BeTrue();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldRenderSavedSearchesList(int savedSearchCount)
        {
            var @partial = new savedSearches();
            var candidateId = Guid.NewGuid();

            var mockViewModel = new Fixture().Build<SavedSearchViewModel>().CreateMany(savedSearchCount).ToList();

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var indexViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches).ViewModel;
            var view = @partial.RenderAsHtml(indexViewModel);

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

        [Test]
        public void ShouldRenderAlertSettingsLink()
        {
            var @partial = new savedSearches();

            var viewModel = Mediator.Index(null, ApprenticeshipSearchMode.SavedSearches).ViewModel;
            var view = @partial.RenderAsHtml(viewModel);

            view.GetElementbyId("saved-searches-settings-link").Should().NotBeNull();
        }
    }
}