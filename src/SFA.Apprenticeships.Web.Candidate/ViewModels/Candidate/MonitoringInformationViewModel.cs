namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public class MonitoringInformationViewModel
    {
        public int? Ethnicity { get; set; }

        public int? Gender { get; set; }

        public int? DisabilityStatus { get; set; }

        public bool RequiresSupportForInterview { get; set; }

        [Display(Name = MonitoringInformationViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.HintText, Description = "")]
        public string AnythingWeCanDoToSupportYourInterview { get; set; }
    }
}