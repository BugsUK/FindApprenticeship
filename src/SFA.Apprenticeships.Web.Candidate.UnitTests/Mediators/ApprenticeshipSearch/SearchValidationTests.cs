namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using Candidate.Mediators.Search;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies.Apprenticeships;
    using NUnit.Framework;

    [TestFixture]
    public class SearchValidationTests : TestsBase
    {
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
        public void SavedSearchesModeValidationError()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                SavedSearchId = string.Empty,
                SearchMode = ApprenticeshipSearchMode.SavedSearches
            };

            var response = Mediator.SearchValidation(null, searchViewModel);

            response.AssertValidationResult(ApprenticeshipSearchMediatorCodes.SearchValidation.ValidationError, true);
        }
    }
}