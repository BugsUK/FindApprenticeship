namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipApplication
{
    using System.Linq;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.MyApplications;
    using Candidate.ViewModels.VacancySearch;
    using Common.Framework;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;

    [TestFixture]
    public class WhatHappensNextTests : ViewUnitTest
    {
        [Test]
        public void ShowsSearchReturnUrlWhenPResent()
        {
            //Arrange
            const string returnUrl = "http://searchreturn";
            var whatHappensNextViewModel = new WhatHappensNextApprenticeshipViewModel
            {
                SuggestedVacanciesSearchViewModel = new ApprenticeshipSearchViewModel()
            };
            var whatHappendNextView = new WhatHappensNextViewBuilder().Build();
            whatHappendNextView.ViewBag.SearchReturnUrl = returnUrl;

            //Act
            var result = whatHappendNextView.RenderAsHtml(whatHappensNextViewModel);

            //Assert
            var searchLink = result.GetElementbyId("search-return-link");
            searchLink.Should().NotBeNull();
            searchLink.GetAttributeValue("href", returnUrl);
        }

        [Test]
        public void ShowPlainFindApprenticeshipButtonWhenNoSavedDraftsOrSuggestedVacancies()
        {
            //Arrange
            var whatHappensNextViewModel = new WhatHappensNextApprenticeshipViewModel();
            var whatHappendNextView = new WhatHappensNextViewBuilder().Build();

            //Act
            var result = whatHappendNextView.RenderAsHtml(whatHappensNextViewModel);

            //Assert
            var searchLink = result.GetElementbyId("find-apprenticeship-btn");
            searchLink.Should().NotBeNull();
            searchLink.InnerText.Should().Be("Find an apprenticeship");
        }

        [Test]
        public void ShowsSuggestedApprenticeshipsAndSuggestedSearchUrl()
        {
            //Arrange            
            var fixture = new Fixture();
            var suggestedViewModels = fixture.CreateMany<SuggestedVacancyViewModel>().ToList();
            var whatHappensNextViewModel = new WhatHappensNextApprenticeshipViewModel
            {
                SuggestedVacancies = suggestedViewModels,
                SuggestedVacanciesSearchViewModel = new ApprenticeshipSearchViewModel() 
            };

            //Act
            var whatHappendNextView = new WhatHappensNextViewBuilder().With(whatHappensNextViewModel).RenderAsHtml();
            whatHappendNextView.Should().NotBeNull();

            //Assert
            for (int i = 0; i < 3; i++)
            {
                var suggestedVac = whatHappendNextView.GetElementbyId("suggested-vacancy-" + suggestedViewModels[i].VacancyId);
                suggestedVac.InnerText.Should().Contain(suggestedViewModels[i].VacancyTitle);
                suggestedVac.InnerText.Should().Contain(suggestedViewModels[i].Distance);
            }
        }

        [Test]
        public void ShowsSavedAndDraftApprenticeshipsAndSuggestedSearchUrl()
        {
            //Arrange            
            var fixture = new Fixture();
            var savedDraftViewModels = fixture.CreateMany<MyApprenticeshipApplicationViewModel>().ToList();
            var suggestedViewModels = fixture.CreateMany<SuggestedVacancyViewModel>().ToList();
            var whatHappensNextViewModel = new WhatHappensNextApprenticeshipViewModel
            {
                SuggestedVacancies = suggestedViewModels,
                SuggestedVacanciesSearchViewModel = new ApprenticeshipSearchViewModel(),
                SavedAndDraftApplications = savedDraftViewModels
            };

            //Act
            var whatHappendNextView = new WhatHappensNextViewBuilder().With(whatHappensNextViewModel).RenderAsHtml();
            whatHappendNextView.Should().NotBeNull();

            //Assert
            for (int i = 0; i < 3; i++)
            {
                var savedDraft = whatHappendNextView.GetElementbyId("saved-vacancy-" + savedDraftViewModels[i].VacancyId);
                var suggestedVac = whatHappendNextView.GetElementbyId("suggested-vacancy-" + suggestedViewModels[i].VacancyId);
                suggestedVac.Should().BeNull();
                savedDraft.Should().NotBeNull();
                savedDraft.InnerText.Should().Contain(savedDraftViewModels[i].Title);
                savedDraft.InnerText.Should().Contain(savedDraftViewModels[i].ClosingDate.ToFriendlyClosingToday());
            }
        }
    }
}