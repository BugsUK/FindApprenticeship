namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Vacancy;

    public class CandidateVacancy
    {
        public VacancyViewModel Vacancy { get; set; }
        public ApplicationViewModel Application { get; set; }
    }
}