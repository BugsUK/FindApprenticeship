namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;

    public interface ICreateDraftApprenticeshipFromSavedVacancyStrategy
    {
        ApprenticeshipApplicationDetail CreateDraft(Guid candidateId, int vacancyId);
    }
}