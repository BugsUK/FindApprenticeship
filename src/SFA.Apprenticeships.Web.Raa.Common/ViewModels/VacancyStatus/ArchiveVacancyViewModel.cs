namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyStatus
{
    public class ArchiveVacancyViewModel
    {
        public ArchiveVacancyViewModel() { }

        public ArchiveVacancyViewModel(bool hasOutstandingActions, int vacancyId, int vacancyReferenceNumber)
        {
            VacancyReferenceNumber = vacancyReferenceNumber;
            HasOutstandingActions = hasOutstandingActions;
            VacancyId = vacancyId;
        }

        public bool HasOutstandingActions { get; set; }

        public int VacancyId { get; set; }

        public int VacancyReferenceNumber { get; set; }
    }
}
