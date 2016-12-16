namespace SFA.DAS.RAA.Api.Controllers
{
    public class VacancyMessages
    {
        public const string MissingVacancyIdentifier = "Please specify either a vacancyId, a vacancyReferenceNumber or a vacancyGuid.";
        public const string VacancyNotFound = "The requested vacancy has not been found.";
        public const string UnauthorizedVacancyAccess = "You are not authorized to view or edit this vacancy.";
    }
}