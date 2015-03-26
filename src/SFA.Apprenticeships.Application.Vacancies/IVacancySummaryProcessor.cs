namespace SFA.Apprenticeships.Application.Vacancies
{
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Messaging;
    using Entities;

    public interface IVacancySummaryProcessor
    {
        void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage);

        void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage);

        void QueueVacancyIfExpiring(ApprenticeshipSummary vacancySummary, int aboutToExpireThreshold);
    }
}
