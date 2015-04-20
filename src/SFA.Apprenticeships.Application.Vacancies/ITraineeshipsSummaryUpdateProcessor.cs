namespace SFA.Apprenticeships.Application.Vacancies
{
    using Entities;

    public interface ITraineeshipsSummaryUpdateProcessor
    {
        void Process(TraineeshipSummaryUpdate vacancySummaryToIndex);
    }
}