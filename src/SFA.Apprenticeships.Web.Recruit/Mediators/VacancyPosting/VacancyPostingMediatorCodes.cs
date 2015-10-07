namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    public static class VacancyPostingMediatorCodes
    {
        public static class GetProviderEmployers
        {
            public const string Ok = "VacancyPosting.GetProviderEmployers.Ok";
        }

        public static class GetEmployers
        {
            public const string Ok = "VacancyPosting.GetEmployers.Ok";
        }

        public static class GetNewVacancyModel
        {
            public const string Ok = "VacancyPosting.GetNewVacancyModel.Ok";
        }

        public static class GetVacancyViewModel
        {
            public const string Ok = "VacancyPosting.GetVacancyViewModel.Ok";
        }

        public static class CreateVacancy
        {
            public const string Ok = "VacancyPosting.CreateVacancy.Ok";
            public const string FailedValidation = "VacancyPosting.CreateVacancy.FailedValidation";
        }

        public static class SubmitVacancy
        {
            public const string Ok = "VacancyPosting.SubmitVacancy.Ok";
            public const string FailedValidation = "VacancyPosting.SubmitVacancy.FailedValidation";
        }

        public static class GetSubmittedVacancyViewModel
        {
            public const string Ok = "VacancyPosting.GetSubmittedVacancyViewModel.Ok";
        }
    }
}
