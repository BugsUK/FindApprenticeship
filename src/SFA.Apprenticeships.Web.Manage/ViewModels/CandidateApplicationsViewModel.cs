namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using Common.ViewModels.MyApplications;

    public class CandidateApplicationsViewModel
    {
        public string CandidateName { get; set; }
        public MyApplicationsViewModel CandidateApplications { get; set; } 
    }
}