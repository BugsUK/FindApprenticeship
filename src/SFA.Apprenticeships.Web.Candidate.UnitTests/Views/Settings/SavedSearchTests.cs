namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using System;
    using System.Linq;
    using System.Security.Policy;
    using Builders;
    using Candidate.ViewModels.Account;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class SavedSearchTests
    {
        [Test]
        public void SavedSearchContainer()
        {
            var viewModel = new SettingsViewModelBuilder().Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var savedSearchDiv = result.GetElementbyId("savedSearch");
            savedSearchDiv.Should().NotBeNull();
            var savedSearchesHeading = result.GetElementbyId("savedSearchHeading");
            savedSearchesHeading.Should().NotBeNull();
            savedSearchesHeading.InnerText.Should().Contain("Saved searches");
        }
        
        [Test]
        public void NoSavedSearches()
        {
            var viewModel = new SettingsViewModelBuilder().Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var savedSearchDiv = result.GetElementbyId("savedSearch");
            savedSearchDiv.Should().NotBeNull();
            var savedSearchesHeading = result.GetElementbyId("savedSearchHeading");
            savedSearchesHeading.Should().NotBeNull();

            var noSavedSearchesText = result.GetElementbyId("noSavedSearchesText");
            noSavedSearchesText.Should().NotBeNull();
            noSavedSearchesText.Attributes.Any(a => a.Name == "style").Should().BeFalse();
            noSavedSearchesText.InnerText.Should().Contain("You currently don't have any active saved searches");
            var savedSearchesDiv = result.GetElementbyId("savedSearches");
            savedSearchesDiv.Should().BeNull();
        }

        [TestCase("All", true)]
        [TestCase("Intermediate", false)]
        [TestCase("Advanced", true)]
        [TestCase("Higher", false)]
        public void OneSavedSearch(string apprenticeshipLevel, bool alertEnabled)
        {
            const string name = "Within 5 miles of CV1 2WT";
            const string searchUrl = "/apprenticeships?Location=CV1%202WT&LocationType=NonNational&ApprenticeshipLevel=All&SearchField=All&SearchMode=Keyword&Hash=0&WithinDistance=5&SortType=Relevancy&PageNumber=1&SearchAction=Search&ResultsPerPage=5";
            var savedSearchViewModels = new Fixture().Build<SavedSearchViewModel>()
                .With(s => s.Name, name)
                .With(s => s.SearchUrl, new Url(searchUrl))
                .With(s => s.AlertsEnabled, alertEnabled)
                .With(s => s.ApprenticeshipLevel, apprenticeshipLevel)
                .With(s => s.SubCategoriesFullNames, string.Empty)
                .CreateMany(1).ToList();
            var viewModel = new SettingsViewModelBuilder().WithSavedSearchViewModels(savedSearchViewModels).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var savedSearchDiv = result.GetElementbyId("savedSearch");
            savedSearchDiv.Should().NotBeNull();
            var savedSearchesHeading = result.GetElementbyId("savedSearchHeading");
            savedSearchesHeading.Should().NotBeNull();

            var noSavedSearchesText = result.GetElementbyId("noSavedSearchesText");
            noSavedSearchesText.Should().NotBeNull();
            noSavedSearchesText.GetAttributeValue("style", string.Empty).Should().Be("display: none");
            var savedSearchesDiv = result.GetElementbyId("savedSearches");
            savedSearchesDiv.Should().NotBeNull();

            var savedSearchElement = savedSearchesDiv.ChildNodes.First(n => n.Name == "div");
            savedSearchElement.Should().NotBeNull();

            var link = savedSearchElement.ChildNodes.First(n => n.Name == "a");
            link.GetAttributeValue("href", string.Empty).Should().Be(searchUrl);
            link.InnerText.Should().Be(name);

            //Last alert span should not be present
            var lastAlertSpanPresent = savedSearchElement.ChildNodes.All(n => n.Name != "span");
            lastAlertSpanPresent.Should().BeFalse();

            var savedSearchPropertyList = savedSearchElement.ChildNodes.First(n => n.Name == "ul");
            var savedSearchProperties = savedSearchPropertyList.ChildNodes.Where(n => n.Name == "li").ToList();

            var shouldShowLevel = apprenticeshipLevel != "All";

            savedSearchProperties.Count.Should().Be(shouldShowLevel ? 3 : 2);

            if (shouldShowLevel)
            {
                savedSearchProperties[0].InnerText.Should().Be(string.Format("Apprenticeship level: {0}", apprenticeshipLevel));
            }

            savedSearchProperties.Last().ChildNodes.First(n => n.Name == "a").InnerText.Should().Contain("Delete saved search");
        }

        [Test]
        public void OneSavedSearchWithSubCategories()
        {
            const string subCategoriesFullName = "Surveying, Construction Civil Engineering";
            var savedSearchViewModels = new Fixture().Build<SavedSearchViewModel>()
                .With(s => s.ApprenticeshipLevel, "All")
                .With(s => s.SubCategoriesFullNames, subCategoriesFullName)
                .CreateMany(1).ToList();
            var viewModel = new SettingsViewModelBuilder().WithSavedSearchViewModels(savedSearchViewModels).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var savedSearchesDiv = result.GetElementbyId("savedSearches");
            savedSearchesDiv.Should().NotBeNull();

            var savedSearchElement = savedSearchesDiv.ChildNodes.First(n => n.Name == "div");
            savedSearchElement.Should().NotBeNull();

            var savedSearchPropertyList = savedSearchElement.ChildNodes.First(n => n.Name == "ul");
            var savedSearchProperties = savedSearchPropertyList.ChildNodes.Where(n => n.Name == "li").ToList();

            savedSearchProperties.Count.Should().Be(3);

            savedSearchProperties[0].InnerText.Should().Be(string.Format("Sub-categories: {0}", subCategoriesFullName));
        }

        [Test]
        public void OneSavedSearchWithDateProcessed()
        {
            var savedSearchViewModels = new Fixture().Build<SavedSearchViewModel>()
                .With(s => s.DateProcessed, DateTime.UtcNow)
                .CreateMany(1).ToList();
            var viewModel = new SettingsViewModelBuilder().WithSavedSearchViewModels(savedSearchViewModels).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var savedSearchesDiv = result.GetElementbyId("savedSearches");
            savedSearchesDiv.Should().NotBeNull();

            var savedSearchElement = savedSearchesDiv.ChildNodes.First(n => n.Name == "div");
            savedSearchElement.Should().NotBeNull();

            var lastAlertSpan = savedSearchElement.ChildNodes.First(n => n.Name == "span");
            lastAlertSpan.InnerText.Should().Be("(Last alert: today)");
        }

        [TestCase(2)]
        [TestCase(3)]
        public void MultipleSavedSearches(int savedSearchCount)
        {
            var savedSearchViewModels = new Fixture().Build<SavedSearchViewModel>().CreateMany(savedSearchCount).ToList();
            var viewModel = new SettingsViewModelBuilder().WithSavedSearchViewModels(savedSearchViewModels).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var savedSearchesDiv = result.GetElementbyId("savedSearches");
            savedSearchesDiv.Should().NotBeNull();

            var savedSearchElements = savedSearchesDiv.ChildNodes.Where(n => n.Name == "div").ToList();
            savedSearchElements.Count.Should().Be(savedSearchCount);
        }
    }
}