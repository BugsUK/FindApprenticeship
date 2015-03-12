namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Communication;

    public interface ISavedSearchAlertRepository
    {
        void Save(SavedSearchAlert savedSearchAlert);

        void Delete(SavedSearchAlert savedSearchAlert);

        Dictionary<Guid, List<SavedSearchAlert>> GetCandidatesSavedSearchAlerts();
    }
}
