namespace SFA.Apprenticeships.Web.Common.ViewModels.Candidate
{
    using System;

    [Serializable]
    public class ApprenticeshipCandidateViewModel : CandidateViewModelBase
    {
        public EducationViewModel Education { get; set; }

        public AboutYouViewModel AboutYou { get; set; }
    }
}