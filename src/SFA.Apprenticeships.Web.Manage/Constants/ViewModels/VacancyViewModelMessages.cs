namespace SFA.Apprenticeships.Web.Manage.Constants.ViewModels
{
    using FluentValidation.Results;

    public class VacancyViewModelMessages
    {
        public const string PostcodeLookupFailed = "There's been a problem processing your request. Please try again.";
        public const string VacancyAuthoredInApi = "This vacancy needs to have all fields validated because it has been uploaded from an approved third party rather than posted through Recruit an apprentice.";
        public const string VacancyAuthoredInAvms = "This vacancy needs to have all fields validated because it has been imported from Apprenticeship vacancies rather than posted through Recruit an apprentice.";
        public const string NoVacanciesAvailble = "All vacancies have been reviewed.";

        public const string NextAvailableVacancy =
            "The first vacancy you chose is already being reviewed by another adviser. This is the next available option.";

        public const string InvalidVacancy = "You've been timed out due to inactivity. This vacancy has now been reviewed by another adviser.";
    }
}