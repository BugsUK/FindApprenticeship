namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;

    public interface IDeleteSavedApprenticeshipVacancyStrategy
    {
        ApprenticeshipApplicationDetail DeleteSavedVacancy(Guid candidateId, int vacancyId);
    }
}