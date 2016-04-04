namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;

    public class CandidateApplicationsViewModel
    {
        public Guid CandidateId { get; set; }
        public string CandidateName { get; set; }
        public IEnumerable<CandidateApprenticeshipApplicationViewModel> CandidateApprenticeshipApplications;
        public IEnumerable<CandidateTraineeshipApplicationViewModel> CandidateTraineeshipApplications;

        public IEnumerable<CandidateApprenticeshipApplicationViewModel> DraftApprenticeshipApplications
        {
            get
            {
                // Draft applications include those where candidate did NOT apply but have been expired or withdrawn.
                return CandidateApprenticeshipApplications
                    .Where(each =>
                        each.ApplicationStatus == ApplicationStatuses.Draft ||
                        each.ApplicationStatus == ApplicationStatuses.Saved ||
                        (each.ApplicationStatus == ApplicationStatuses.ExpiredOrWithdrawn && !each.DateApplied.HasValue))
                     .OrderBy(app => app.ClosingDate);
            }
        }
        public IEnumerable<CandidateApprenticeshipApplicationViewModel> SubmittedApprenticeshipApplications
        {
            get
            {
                return CandidateApprenticeshipApplications.Where(each =>
                    each.ApplicationStatus == ApplicationStatuses.Submitting ||
                    each.ApplicationStatus == ApplicationStatuses.Submitted ||
                    each.ApplicationStatus == ApplicationStatuses.InProgress);
            }
        }

        public IEnumerable<CandidateApprenticeshipApplicationViewModel> SuccessfulApprenticeshipApplications
        {
            get { return CandidateApprenticeshipApplications.Where(each => each.ApplicationStatus == ApplicationStatuses.Successful); }
        }

        public IEnumerable<CandidateApprenticeshipApplicationViewModel> UnsuccessfulApplications
        {
            get
            {
                // Unsuccessful applications include those where candidate applied but have been expired or withdrawn.
                return CandidateApprenticeshipApplications
                    .Where(each =>
                        each.ApplicationStatus == ApplicationStatuses.Unsuccessful ||
                        (each.ApplicationStatus == ApplicationStatuses.ExpiredOrWithdrawn && each.DateApplied.HasValue));
            }
        }

        public string NextStepUrl { get; set; }
    }
}