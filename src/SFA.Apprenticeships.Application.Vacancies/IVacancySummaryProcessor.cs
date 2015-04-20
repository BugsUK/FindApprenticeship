namespace SFA.Apprenticeships.Application.Vacancies
{
    using Domain.Interfaces.Messaging;

    public interface IVacancySummaryProcessor
    {
        void ProcessVacancyPages(StorageQueueMessage scheduledQueueMessage);
    }
}
