namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System;

    public class SubmittedVacancyViewModel
    {
        public long VacancyReferenceNumber { get; set; }
        public string ApproverEmail { get; set; }
        public DateTime PublishDate { get; set; }
    }
}