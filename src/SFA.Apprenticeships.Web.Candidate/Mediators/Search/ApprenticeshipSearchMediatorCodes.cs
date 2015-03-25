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

        public class RunSavedSearch
        {
            public const string Ok = "ApprenticeshipSearch.RunSavedSearch.Ok";
            public const string SavedSearchNotFound = "ApprenticeshipSearch.RunSavedSearch.SavedSearchNotFound";
            public const string RunSaveSearchFailed = "ApprenticeshipSearch.RunSavedSearch.RunSaveSearchFailed";
        }
    }
}
