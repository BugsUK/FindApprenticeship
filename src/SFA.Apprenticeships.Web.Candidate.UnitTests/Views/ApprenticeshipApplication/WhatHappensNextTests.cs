namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipApplication
{
    using System.Linq;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.VacancySearch;
    using FluentAssertions;
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
        public void ShowsFindApprenticeshipButtonIfNoSuggestedVacancies()
        {
            //Arrange
            var whatHappensNextViewModel = new WhatHappensNextApprenticeshipViewModel
            {
                SuggestedVacanciesSearchViewModel = new ApprenticeshipSearchViewModel()
            };
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
    }
}