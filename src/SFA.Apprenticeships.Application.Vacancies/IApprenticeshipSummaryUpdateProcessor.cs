namespace SFA.Apprenticeships.Application.Vacancies
{
    using Entities;

    public interface IApprenticeshipSummaryUpdateProcessor
    {
        void Process(ApprenticeshipSummaryUpdate vacancySummaryToIndex);
    }
}