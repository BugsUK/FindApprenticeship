namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using Vacancies.Traineeships;

    public class TraineeshipApplicationDetail : ApplicationDetail
    {
        public TraineeshipApplicationDetail()
        {
            Vacancy = new TraineeshipSummary();
        }

        public TraineeshipSummary Vacancy { get; set; }
    }
}