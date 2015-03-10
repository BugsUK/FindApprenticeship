﻿namespace SFA.Apprenticeships.Application.Applications
{
    using Entities;

    public interface IApplicationStatusProcessor
    {
        void QueueApplicationStatusesPages();

        void QueueApplicationStatuses(ApplicationUpdatePage applicationStatusSummaryPage);

        void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary);

        //TODO: 1.6: This should probably be in a separate interface as it's also used outside of Application ETL
        void ProcessApplicationStatuses(VacancyStatusSummary vacancyStatusSummary);
    }
}
