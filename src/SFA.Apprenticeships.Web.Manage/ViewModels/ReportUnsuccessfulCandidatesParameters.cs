namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System;

    public class ReportUnsuccessfulCandidatesParameters
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Type { get; set; }

        public string AgeRange { get; set; }
    }
}