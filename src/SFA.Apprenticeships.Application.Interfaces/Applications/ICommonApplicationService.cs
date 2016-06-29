namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    using Domain.Entities.Applications;
    using System;
    using System.Collections.Generic;

    public interface ICommonApplicationService
    {
        int GetApplicationCount(int vacancyId);

        void UpdateApplicationNotes(Guid applicationId, string notes);
        
        IReadOnlyDictionary<int, IApplicationCounts> GetCountsForVacancyIds(IEnumerable<int> vacancyIds);
    }
}
}