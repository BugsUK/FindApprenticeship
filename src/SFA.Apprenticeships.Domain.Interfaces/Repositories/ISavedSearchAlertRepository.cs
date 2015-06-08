namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Candidates;
    using Entities.Communication;

    public interface ISavedSearchAlertRepository
    {
        SavedSearchAlert Get(Guid id);

        SavedSearchAlert GetUnsentSavedSearchAlert(SavedSearch savedSearch);

        void Save(SavedSearchAlert savedSearchAlert);

        void Delete(SavedSearchAlert savedSearchAlert);

        Dictionary<Guid, List<SavedSearchAlert>> GetCandidatesSavedSearchAlerts();
        
        IEnumerable<Guid> GetAlertsCreatedOnOrBefore(DateTime dateTime);
    }
}
