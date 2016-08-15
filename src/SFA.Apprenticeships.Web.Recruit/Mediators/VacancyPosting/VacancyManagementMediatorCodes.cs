namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    public class VacancyManagementMediatorCodes
    {
        public class ConfirmDelete
        {
            public const string Ok = "VacancyManagement.ConfirmDelete.Ok";
        }

        public class DeleteVacancy
        {
            public const string Ok = "VacancyPosting.DeleteVacancy.Ok";
            public const string VacancyInIncorrectState = "VacancyPosting.DeleteVacancy.VacancyInIncorrectState";
        }
    }
}