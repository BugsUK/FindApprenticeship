namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    public class ApprenticeshipSearchMediatorCodes
    {
        public class Index
        {
            public const string Ok = "ApprenticeshipSearch.Index.Ok";
        }

        public class SearchValidation
        {
            public const string Ok = "ApprenticeshipSearch.SearchValidation.Ok";
            public const string ValidationError = "ApprenticeshipSearch.SearchValidation.ValidationError";
            public const string CandidateNotLoggedIn = "ApprenticeshipSearch.SearchValidation.CandidateNotLoggedIn";
            public const string RunSavedSearch = "ApprenticeshipSearch.SearchValidation.RunSavedSearch";
        }

        public class Results
        {
            public const string HasError = "ApprenticeshipSearch.Results.HasError";
            public const string Ok = "ApprenticeshipSearch.Results.Ok";
            public const string ValidationError = "ApprenticeshipSearch.Results.ValidationError";
            public const string ExactMatchFound = "ApprenticeshipSearch.Results.ExactMatchFound";
        }

        public class SaveSearch
        {
            public const string HasError = "ApprenticeshipSearch.SaveSearch.HasError";
            public const string Ok = "ApprenticeshipSearch.SaveSearch.Ok";
        }

        public class Details
        {
            public const string VacancyNotFound = "ApprenticeshipSearch.Details.VacancyNotFound";
            public const string VacancyHasError = "ApprenticeshipSearch.Details.VacancyHasError";
            public const string Ok = "ApprenticeshipSearch.Details.Ok";
        }

        public class SavedSearch
        {
            public const string Ok = "ApprenticeshipSearch.SavedSearch.Ok";
            public const string SavedSearchNotFound = "ApprenticeshipSearch.SavedSearch.SavedSearchNotFound";
            public const string RunSaveSearchFailed = "ApprenticeshipSearch.SavedSearch.RunSaveSearchFailed";
        }

        public class RedirectToExternalWebsite
        {
            public const string VacancyNotFound = "ApprenticeshipSearch.RedirectToExternalWebsite.VacancyNotFound";
            public const string VacancyHasError = "ApprenticeshipSearch.RedirectToExternalWebsite.VacancyHasError";
            public const string Ok = "ApprenticeshipSearch.RedirectToExternalWebsite.Ok";
        }
    }
}
