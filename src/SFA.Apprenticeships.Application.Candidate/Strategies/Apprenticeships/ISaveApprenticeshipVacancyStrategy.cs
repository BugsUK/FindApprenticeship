namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;

    public interface ISaveApprenticeshipVacancyStrategy
    {
        ApprenticeshipApplicationDetail SaveVacancy(Guid candidateId, int vacancyId);
    }
}