namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using Entities;

    //todo: 1.8: move to Candidates? TBC
    public interface ISavedSearchProcessor
    {
        void QueueCandidateSavedSearches();

        void ProcessCandidateSavedSearches(CandidateSavedSearches candidateSavedSearches);
    }
}
