namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public class MonitoringInformationViewModel
    {
        public int? Ethnicity { get; set; }

        public int? Gender { get; set; }

        public int? DisabilityStatus { get; set; }

        public string DisabilityStatusDescription
        {
            get
            {
                if (!DisabilityStatus.HasValue)
                {
                    return null;
                }

                switch (DisabilityStatus.Value)
                {
                    case 1:
                        return "Yes";
                    case 2:
                        return "No";
                    case 3:
                        return "Prefer not to say";
                }

                return null;
            }
        }

        public bool RequiresSupportForInterview { get; set; }

        [Display(Name = MonitoringInformationViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.HintText, Description = "")]
        public string AnythingWeCanDoToSupportYourInterview { get; set; }
    }
}