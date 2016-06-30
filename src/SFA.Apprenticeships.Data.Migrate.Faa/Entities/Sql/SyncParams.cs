namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class SyncParams
    {
        public int? LastSyncVersion { get; set; }
        public DateTime ApprenticeshipLastCreatedDate { get; set; }
        public DateTime ApprenticeshipLastUpdatedDate { get; set; }
        public DateTime TraineeshipLastCreatedDate { get; set; }
        public DateTime TraineeshipLastUpdatedDate { get; set; }
        public DateTime CandidateLastCreatedDate { get; set; }
        public DateTime CandidateLastUpdatedDate { get; set; }
        public bool IsValidForApprenticeshipIncrementalSync => ApprenticeshipLastCreatedDate != DateTime.MinValue && ApprenticeshipLastUpdatedDate != DateTime.MinValue;
        public bool IsValidForTraineeshipIncrementalSync => TraineeshipLastCreatedDate != DateTime.MinValue && TraineeshipLastUpdatedDate != DateTime.MinValue;
        public bool IsValidForCandidateIncrementalSync => CandidateLastCreatedDate != DateTime.MinValue && CandidateLastUpdatedDate != DateTime.MinValue;

        public override string ToString()
        {
            return $"ApprenticeshipLastCreatedDate: {ApprenticeshipLastCreatedDate}, ApprenticeshipLastUpdatedDate: {ApprenticeshipLastUpdatedDate}, TraineeshipLastCreatedDate: {TraineeshipLastCreatedDate}, TraineeshipLastUpdatedDate: {TraineeshipLastUpdatedDate}, CandidateLastCreatedDate: {CandidateLastCreatedDate}, CandidateLastUpdatedDate: {CandidateLastUpdatedDate}";
        }
    }
}
