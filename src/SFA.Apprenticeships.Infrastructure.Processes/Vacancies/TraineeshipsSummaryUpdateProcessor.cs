namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using VacancyIndexer;
    using Elastic = Elastic.Common.Entities;

    public class TraineeshipsSummaryUpdateProcessor : ITraineeshipsSummaryUpdateProcessor
    {
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, Elastic.TraineeshipSummary> _vacancyIndexerService;

        public TraineeshipsSummaryUpdateProcessor(IVacancyIndexerService<TraineeshipSummaryUpdate, Elastic.TraineeshipSummary> vacancyIndexerService)
        {
            _vacancyIndexerService = vacancyIndexerService;
        }

        public void Process(TraineeshipSummaryUpdate vacancySummaryToIndex)
        {
            _vacancyIndexerService.Index(vacancySummaryToIndex);
        }
    }
}
