namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    public class VacancyMediatorCodes
    {
        public class ApproveVacancy
        {
            public const string NoAvailableVacancies = "VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies";
            public const string Ok = "VacancyMediatorCodes.ApproveVacancy.Ok";
        }

        public class RejectVacancy
        {
            public const string NoAvailableVacancies = "VacancyMediatorCodes.RejectVacancy.NoAvailableVacancies";
            public const string Ok = "VacancyMediatorCodes.RejectVacancy.Ok";
        }

        public class GetVacancy
        {
            public const string NotAvailable = "VacancyMediatorCodes.GetVacancy.NotAvailable";
            public const string Ok = "VacancyMediatorCodes.GetVacancy.Ok";
            public const string FailedValidation = "VacancyMediatorCodes.GetVacancy.FailedValidation";
        }

        public class GetVacancySummaryViewModel
        {
            public const string FailedValidation = "VacancyMediatorCodes.GetVacancySummaryViewModel.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.GetVacancySummaryViewModel.Ok";
        }
        
        public class UpdateVacancy
        {
            public const string FailedValidation = "VacancyMediatorCodes.UpdateVacancy.FailedValidation";
            public const string Ok = "VacancyMediatorCodes.UpdateVacancy.Ok";
        }
        
        public class GetBasicVacancyDetails
        {
            public const string NotAvailable = "VacancyMediatorCodes.GetBasicVacancyDetails.NotAvailable";
            public const string Ok = "VacancyMediatorCodes.GetBasicVacancyDetails.Ok";
            public const string FailedValidation = "VacancyMediatorCodes.GetBasicVacancyDetails.FailedValidation";
        }


    }
}