namespace SFA.DAS.RAA.Api.Constants
{
    public class VacancyMessages
    {
        public const string MissingVacancyIdentifier = "Please specify either an Id, a Reference or a Guid for your requested vacancy.";
        public const string InvalidVacancyReference = "The supplied vacancy reference is invalid.";
        public const string VacancyNotFound = "The requested vacancy has not been found.";
        public const string UnauthorizedVacancyAccess = "You are not authorized to view or edit this vacancy.";
    }
}