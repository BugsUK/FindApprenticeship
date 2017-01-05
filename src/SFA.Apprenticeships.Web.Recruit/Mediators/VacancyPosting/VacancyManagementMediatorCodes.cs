namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    public class VacancyManagementMediatorCodes
    {
        public class ConfirmDelete
        {
            public const string Ok = "VacancyManagement.ConfirmDelete.Ok";
            public const string NotFound = "VacancyManagement.ConfirmDelete.NotFound";
        }

        public class DeleteVacancy
        {
            public const string Ok = "VacancyPosting.DeleteVacancy.Ok";
            public const string Failure = "VacancyPosting.DeleteVacancy.Failure";
        }

        public class EditWage
        {
            public const string Ok = "VacancyPosting.EditWage.Ok";
            public const string NotFound = "VacancyManagement.EditWage.NotFound";
            public const string FailedValidation = "VacancyPosting.EditWage.FailedValidation";
            public const string Failure = "VacancyPosting.EditWage.Failure";
        }
    }
}