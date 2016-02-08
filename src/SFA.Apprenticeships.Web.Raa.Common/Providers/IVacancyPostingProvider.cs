namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ViewModels.Provider;
    using ViewModels.ProviderUser;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IVacancyPostingProvider
    {
        NewVacancyViewModel GetNewVacancyViewModel(int vacancyReferenceNumber);

        NewVacancyViewModel GetNewVacancyViewModel(string ukprn, string providerSiteErn, string ern, Guid vacancyGuid, int? numberOfPositions);

        NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        TrainingDetailsViewModel GetTrainingDetailsViewModel(int vacancyReferenceNumber);

        TrainingDetailsViewModel UpdateVacancy(TrainingDetailsViewModel viewModel);

        VacancySummaryViewModel GetVacancySummaryViewModel(int vacancyReferenceNumber);

        VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(int vacancyReferenceNumber);

        VacancyQuestionsViewModel GetVacancyQuestionsViewModel(int vacancyReferenceNumber);

        VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel);

        VacancySummaryViewModel UpdateVacancy(VacancySummaryViewModel viewModel);

        VacancyRequirementsProspectsViewModel UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);

        VacancyViewModel GetVacancy(int vacancyReferenceNumber);

        VacancyViewModel SubmitVacancy(int vacancyReferenceNumber);

        List<SelectListItem> GetSectorsAndFrameworks();

        List<StandardViewModel> GetStandards();

        StandardViewModel GetStandard(int? standardId);

        VacanciesSummaryViewModel GetVacanciesSummaryForProvider(string ukprn, string providerSiteErn, VacanciesSummarySearchViewModel vacanciesSummarySearch);

        ProviderSiteEmployerLinkViewModel CloneVacancy(int vacancyReferenceNumber);

        LocationSearchViewModel CreateVacancy(LocationSearchViewModel newVacancyViewModel);

        LocationSearchViewModel LocationAddressesViewModel(string ukprn, string providerSiteErn, string ern, Guid vacancyGuid);

        VacancyViewModel GetVacancy(Guid vacancyReferenceNumber);

        void RemoveVacancyLocationInformation(Guid vacancyGuid);

        void RemoveLocationAddresses(Guid vacancyGuid);

        LocationSearchViewModel AddLocations(LocationSearchViewModel viewModel);

        VacancyDatesViewModel GetVacancyDatesViewModel(int vacancyReferenceNumber);

        VacancyDatesViewModel UpdateVacancy(VacancyDatesViewModel viewModel);
    }
}
