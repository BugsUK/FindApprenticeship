namespace SFA.Apprenticeships.Web.Candidate.ViewModels.MyApplications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Applications;
    using Domain.Entities.Applications;

    public class MyApplicationsViewModel
    {
        public const string PartialView = "MyApplications";

        public MyApplicationsViewModel(
            IEnumerable<MyApprenticeshipApplicationViewModel> apprenticeshipApplications,
            IEnumerable<MyTraineeshipApplicationViewModel> traineeshipApplications,
            TraineeshipFeatureViewModel traineeshipFeature,
            DateTime? lastApplicationStatusNotification)
        {
            AllApprenticeshipApplications = apprenticeshipApplications
                .Where(a => !a.IsArchived)
                .OrderByDescending(a => a.DateUpdated);

            TraineeshipApplications = traineeshipApplications
                .Where(a => !a.IsArchived)
                .OrderByDescending(a => a.DateApplied);

            TraineeshipFeature = traineeshipFeature;

            LastApplicationStatusNotification = lastApplicationStatusNotification;
        }

        public DateTime? LastApplicationStatusNotification { get; private set; }

        public IEnumerable<MyApprenticeshipApplicationViewModel> AllApprenticeshipApplications { get; private set; }

        public IEnumerable<MyTraineeshipApplicationViewModel> TraineeshipApplications { get; private set; }

        public TraineeshipFeatureViewModel TraineeshipFeature { get; set; }

        public IEnumerable<MyApprenticeshipApplicationViewModel> SubmittedApprenticeshipApplications
        {
            get
            {
                return AllApprenticeshipApplications.Where(each =>
                    each.ApplicationStatus == ApplicationStatuses.Submitting ||
                    each.ApplicationStatus == ApplicationStatuses.Submitted ||
                    each.ApplicationStatus == ApplicationStatuses.InProgress);
            }
        }

        public IEnumerable<MyApprenticeshipApplicationViewModel> SuccessfulApprenticeshipApplications
        {
            get { return AllApprenticeshipApplications.Where(each => each.ApplicationStatus == ApplicationStatuses.Successful); }
        }

        public IEnumerable<MyApprenticeshipApplicationViewModel> UnsuccessfulApplications
        {
            get
            {
                // Unsuccessful applications include those where candidate applied but have been expired or withdrawn.
                return AllApprenticeshipApplications
                    .Where(each =>
                        each.ApplicationStatus == ApplicationStatuses.Unsuccessful ||
                        (each.ApplicationStatus == ApplicationStatuses.ExpiredOrWithdrawn && each.DateApplied.HasValue));
            }
        }


        public IEnumerable<MyApprenticeshipApplicationViewModel> ApplicationStatusNotifications
        {
            get
            {
                if (!LastApplicationStatusNotification.HasValue)
                {
                    return Enumerable.Empty<MyApprenticeshipApplicationViewModel>();
                }

                return
                    SuccessfulApprenticeshipApplications.Union(UnsuccessfulApplications)
                        .Where(a => a.DateUpdated >= LastApplicationStatusNotification.Value)
                        .OrderByDescending(a => a.DateApplied);
            }
        }

        public long ApplicationStatusNotificationsLastUpdatedDateTimeTicks
        {
            get
            {
                var lastAppUpdated = ApplicationStatusNotifications.FirstOrDefault();
                return lastAppUpdated == null ? 0 : lastAppUpdated.DateUpdated.Ticks;
            }
        }

        public IEnumerable<MyApprenticeshipApplicationViewModel> DraftApprenticeshipApplications
        {
            get
            {
                // Draft applications include those where candidate did NOT apply but have been expired or withdrawn.
                return AllApprenticeshipApplications
                    .Where(each =>
                        each.ApplicationStatus == ApplicationStatuses.Draft ||
                        each.ApplicationStatus == ApplicationStatuses.Saved ||
                        (each.ApplicationStatus == ApplicationStatuses.ExpiredOrWithdrawn && !each.DateApplied.HasValue))
                     .OrderBy(app => app.ClosingDate);
            }
        }

        public string DeletedVacancyId { get; set; }

        public string DeletedVacancyTitle { get; set; }
    }
}
