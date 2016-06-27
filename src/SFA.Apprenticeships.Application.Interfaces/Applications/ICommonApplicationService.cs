namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    using System;
    using System.Collections.Generic;

    public interface ICommonApplicationService
    {
        int GetApplicationCount(int vacancyId);

        int GetNewApplicationCount(int vacancyId);

        void UpdateApplicationNotes(Guid applicationId, string notes);
        
        int GetNewApplicationsCount(List<int> liveVacancyIds);
    }
}