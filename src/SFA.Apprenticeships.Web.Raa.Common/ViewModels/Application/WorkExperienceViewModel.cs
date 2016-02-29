namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using System;

    public class WorkExperienceViewModel
    {
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}