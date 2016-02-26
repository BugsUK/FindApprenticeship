namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Common.ViewModels.VacancySearch;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipVacancyProvider
    {
        ApprenticeshipSearchResponseViewModel FindVacancies(ApprenticeshipSearchViewModel search);

        ApprenticeshipVacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId);

        ApprenticeshipVacancyDetailViewModel IncrementClickThroughFor(int vacancyId);
    }
}