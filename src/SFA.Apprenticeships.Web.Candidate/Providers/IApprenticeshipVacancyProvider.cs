namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipVacancyProvider
    {
        ApprenticeshipSearchResponseViewModel FindVacancies(ApprenticeshipSearchViewModel search);

        ApprenticeshipVacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId);

        ApprenticeshipVacancyDetailViewModel GetVacancyDetailViewModelByReferenceNumber(Guid? candidateId, int vacancyReferenceNumber);

        ApprenticeshipVacancyDetailViewModel IncrementClickThroughFor(int vacancyId);
    }
}