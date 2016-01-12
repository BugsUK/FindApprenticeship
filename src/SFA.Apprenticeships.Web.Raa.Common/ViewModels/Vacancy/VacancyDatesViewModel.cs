namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
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

        public long VacancyReferenceNumber { get; set; }

        public UpdateVacancyDatesState State { get; set; }
    }

    public enum UpdateVacancyDatesState
    {
        UpdatedHasApplications,
        UpdatedNoApplications,
        InvalidState
    }
}