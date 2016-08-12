namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyStatus
{
    using System;

    public class ArchiveVacancyViewModel
    {
        public ArchiveVacancyViewModel() { }

        public ArchiveVacancyViewModel(bool hasOutstandingActions, int vacancyId, int vacancyReferenceNumber)
        {
            if (vacancyId == 0 || vacancyReferenceNumber == 0)
            {
                throw new ArgumentNullException();
            }

            VacancyReferenceNumber = vacancyReferenceNumber;
            HasOutstandingActions = hasOutstandingActions;
            VacancyId = vacancyId;
        }

        public bool HasOutstandingActions { get; set; }

        public int VacancyId { get; set; }

        public int VacancyReferenceNumber { get; set; }
    }
}
