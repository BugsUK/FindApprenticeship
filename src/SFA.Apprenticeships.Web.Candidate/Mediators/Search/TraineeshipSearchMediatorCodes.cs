namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    public class TraineeshipSearchMediatorCodes
    {
        public class Index
        {
            public const string Ok = "TraineeshipSearch.Index.Ok";
        }

        public class Results
        {
            public const string Ok = "TraineeshipSearch.Results.Ok";
            public const string ValidationError = "TraineeshipSearch.Results.ValidationError";
            public const string HasError = "TraineeshipSearch.Results.HasError";
            public const string ExactMatchFound = "TraineeshipSearch.Results.ExactMatchFound";
        }

        public class SearchValidation
        {
            public const string Ok = "TraineeshipSearch.SearchValidation.Ok";
            public const string ValidationError = "TraineeshipSearch.SearchValidation.ValidationError";
            public const string HasError = "TraineeshipSearch.SearchValidation.HasError";
        }

        public class Details
        {
            public const string Ok = "TraineeshipSearch.Details.Ok";
            public const string VacancyNotFound = "TraineeshipSearch.Details.VacancyNotFound";
            public const string VacancyHasError = "TraineeshipSearch.Details.VacancyHasError";
        }
    }
}
