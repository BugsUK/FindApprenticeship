namespace SFA.Apprenticeships.Web.Common.ViewModels.Candidate
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Domain.Entities.Candidates;

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

                var disabilityStatus = (DisabilityStatus) DisabilityStatus.Value;

                switch (disabilityStatus)
                {
                    case Domain.Entities.Candidates.DisabilityStatus.Yes:
                        return "Yes";
                    case Domain.Entities.Candidates.DisabilityStatus.No:
                        return "No";
                    case Domain.Entities.Candidates.DisabilityStatus.PreferNotToSay:
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