namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Web.Common.ViewModels;

    public class VacancyDatesViewModel
    {
        [Display(Name = VacancyViewModelMessages.ClosingDate.LabelText)]
        public DateViewModel ClosingDate { get; set; }

        [Display(Name = VacancyViewModelMessages.ClosingDateComment.LabelText)]
        public string ClosingDateComment { get; set; }

        [Display(Name = VacancyViewModelMessages.PossibleStartDate.LabelText)]
        public DateViewModel PossibleStartDate { get; set; }

        [Display(Name = VacancyViewModelMessages.PossibleStartDateComment.LabelText)]
        public string PossibleStartDateComment { get; set; }

        public int WarningsHash { get; set; }

        public int VacancyReferenceNumber { get; set; }

        public VacancyStatus VacancyStatus { get; set; }

        public VacancyApplicationsState VacancyApplicationsState { get; set; }

        public int AutoSaveTimeoutInSeconds { get; set; }
    }
}