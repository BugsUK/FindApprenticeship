namespace SFA.Apprenticeships.Application.Candidate.Strategies.SuggestedVacancies
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Interfaces.Search;
    using Interfaces.Vacancies;

    public interface IApprenticeshipVacancySuggestionsStrategy
    {
        SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> GetSuggestedApprenticeshipVacancies(
            ApprenticeshipSearchParameters searchParameters, IList<ApprenticeshipApplicationSummary> candidateApplications, int vacancyId);
    }
}
