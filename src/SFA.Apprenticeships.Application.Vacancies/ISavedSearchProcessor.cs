namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using Entities;

    public interface ISavedSearchProcessor
    {
        void QueueSavedSearchCandidates();

        void ProcessSavedSearchCandidate(CandidateSavedSearches candidateSavedSearches);
    }
}
