namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Report
{
    using System.Collections.Generic;
    using Constants;

    public class ReportMenu
    {
        public Dictionary<string, string> ReportList { get; private set; }

        public ReportMenu()
        {
            ReportList = new Dictionary<string, string>
            {
                {"Applications Received", RecruitmentRouteNames.ReportApplicationsReceived},
                {"Candidates with Applications", RecruitmentRouteNames.ReportCandidatesWithApplications}
            };
        }
    }
}