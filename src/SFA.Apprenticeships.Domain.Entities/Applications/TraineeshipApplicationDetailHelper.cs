namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using System.Linq;
    using Exceptions;

    public static class TraineeshipApplicationDetailHelper
    {
        public static void AssertState(this TraineeshipApplicationDetail traineeshipApplicationDetail, string errorMessage, params ApplicationStatuses[] allowedUserStatuses)
        {
            if (!allowedUserStatuses.Contains(traineeshipApplicationDetail.Status))
            {
                var expectedStatuses = string.Join(", ", allowedUserStatuses);
                var message = string.Format("Application in invalid state for '{0}' (id: {1}, current: '{2}', expected: '{3}')",
                    errorMessage,
                    traineeshipApplicationDetail.EntityId,
                    traineeshipApplicationDetail.Status,
                    expectedStatuses);

                throw new CustomException(message, ErrorCodes.EntityStateError);
            }
        }

        public static void SetStateSubmitting(this TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            traineeshipApplicationDetail.Status = ApplicationStatuses.Submitting;
            traineeshipApplicationDetail.DateApplied = DateTime.UtcNow;
        }

        public static void SetStateSubmitted(this TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            traineeshipApplicationDetail.Status = ApplicationStatuses.Submitted;
        }

        public static void SetStateInProgress(this TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            traineeshipApplicationDetail.Status = ApplicationStatuses.InProgress;
            traineeshipApplicationDetail.DateLastViewed = DateTime.UtcNow;
        }

        public static void SetStateExpiredOrWithdrawn(this TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            traineeshipApplicationDetail.Status = ApplicationStatuses.ExpiredOrWithdrawn;
        }

        public static void RevertStateToDraft(this TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            traineeshipApplicationDetail.Status = ApplicationStatuses.Draft;
            traineeshipApplicationDetail.DateApplied = null;
        }
    }
}