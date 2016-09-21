namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IGetCandidateVacancyDetailStrategy<out TVacancyDetail>
        where TVacancyDetail : VacancyDetail
    {
        TVacancyDetail GetVacancyDetails(Guid candidateId, int vacancyId);
    }
}
