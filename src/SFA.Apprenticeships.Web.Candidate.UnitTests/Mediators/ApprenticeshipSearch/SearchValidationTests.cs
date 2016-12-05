
namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using Candidate.Mediators.Search;
    using Candidate.ViewModels.VacancySearch;
    using Common.UnitTests.Mediators;
    using Domain.Entities.Vacancies;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class SearchValidationTests : TestsBase
    {

        [Test]
        public void CategoryModeValidationError()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Category = string.Empty,
                Location = string.Empty,
                SearchMode = ApprenticeshipSearchMode.Category
            };

            var response = Mediator.SearchValidation(null, searchViewModel);

            response.AssertValidationResult(ApprenticeshipSearchMediatorCodes.SearchValidation.ValidationError, true);
        }
        [Test]
        public void KeywordModeValidationError()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = string.Empty,
                Location = string.Empty,
                SearchMode = ApprenticeshipSearchMode.Keyword
            };

            var response = Mediator.SearchValidation(null, searchViewModel);

            response.AssertValidationResult(ApprenticeshipSearchMediatorCodes.SearchValidation.ValidationError, true);
        }

        [Test]
        public void SavedSearchesModeCandidateNotLoggedIn()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                SavedSearchId = Guid.NewGuid().ToString(),
                SearchMode = ApprenticeshipSearchMode.SavedSearches
            };

            var response = Mediator.SearchValidation(null, searchViewModel);

            response.AssertCodeAndMessage(ApprenticeshipSearchMediatorCodes.SearchValidation.CandidateNotLoggedIn);
        }

        [Test]
        public void SavedSearchesModeRunSavedSearch()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                SavedSearchId = Guid.NewGuid().ToString(),
                SearchMode = ApprenticeshipSearchMode.SavedSearches
            };

            var response = Mediator.SearchValidation(Guid.NewGuid(), searchViewModel);

            response.AssertCodeAndMessage(ApprenticeshipSearchMediatorCodes.SearchValidation.RunSavedSearch);
        }

        [Test]
        public void SavedSearchesModeValidationError()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                SavedSearchId = string.Empty,
                SearchMode = ApprenticeshipSearchMode.SavedSearches
            };

            var response = Mediator.SearchValidation(Guid.NewGuid(), searchViewModel);

            response.AssertValidationResult(ApprenticeshipSearchMediatorCodes.SearchValidation.ValidationError, true);
        }
    }
}