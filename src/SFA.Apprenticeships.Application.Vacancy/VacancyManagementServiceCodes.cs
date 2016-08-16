namespace SFA.Apprenticeships.Application.Vacancy
{
    public class VacancyManagementServiceCodes
    {
        public class Delete
        {
            public const string Ok = "VacancyManagement.Delete.Ok";
            public const string VacancyInIncorrectState = "Vacancy.Delete.VacancyInIncorrectState";
            public const string VacancyNotFound = "Vacancy.Delete.VacancyNotFound";
        }

        public class FindSummary
        {
            public const string Ok = "VacancyManagement.FindSummary.Ok";
            public const string NotFound = "VacancyManagement.FindSummary.NotFound";
        }
    }
}