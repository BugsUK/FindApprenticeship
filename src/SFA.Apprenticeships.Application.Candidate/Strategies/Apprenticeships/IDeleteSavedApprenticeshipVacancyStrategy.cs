namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;

    public interface IDeleteSavedApprenticeshipVacancyStrategy
    {
        ApprenticeshipApplicationDetail DeletedSavedVacancy(Guid candidateId, int vacancyId);
    }
}