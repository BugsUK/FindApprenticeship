namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Candidate
{
    using Application;
    using Web.Common.ViewModels;

    public class CandidateApplicationSummariesViewModel
    {
        public CandidateApplicationsSearchViewModel CandidateApplicationsSearch { get; set; }

        public ApplicantDetailsViewModel ApplicantDetails { get; set; }

        public PageableViewModel<CandidateApplicationSummaryViewModel> ApplicationSummaries { get; set; }
    }
}