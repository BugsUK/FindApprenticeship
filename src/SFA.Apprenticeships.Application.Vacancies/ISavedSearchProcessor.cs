namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using Entities;

    public interface ISavedSearchProcessor
    {
        void QueueCandidateSavedSearches();

        void ProcessCandidateSavedSearches(CandidateSavedSearches candidateSavedSearches);
    }
}
