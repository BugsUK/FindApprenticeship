namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ViewModels.Admin;
    using ViewModels.Provider;
    using ViewModels.ProviderUser;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IVacancyPostingProvider
    {
        NewVacancyViewModel GetNewVacancyViewModel(int vacancyReferenceNumber);

        NewVacancyViewModel GetNewVacancyViewModel(int vacancyOwnerRelationshipId, Guid vacancyGuid, int? numberOfPositions);

        NewVacancyViewModel UpdateVacancy(NewVacancyViewModel newVacancyViewModel);

        VacancyMinimumData UpdateVacancy(VacancyMinimumData vacancyMinimumData);

        TrainingDetailsViewModel GetTrainingDetailsViewModel(int vacancyReferenceNumber);

        TrainingDetailsViewModel UpdateVacancy(TrainingDetailsViewModel viewModel);

        FurtherVacancyDetailsViewModel GetVacancySummaryViewModel(int vacancyReferenceNumber);

        VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(int vacancyReferenceNumber);

        VacancyQuestionsViewModel GetVacancyQuestionsViewModel(int vacancyReferenceNumber);

        VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel);

        FurtherVacancyDetailsViewModel UpdateVacancy(FurtherVacancyDetailsViewModel viewModel);

        FurtherVacancyDetailsViewModel UpdateVacancyDates(FurtherVacancyDetailsViewModel viewModel);

        VacancyRequirementsProspectsViewModel UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);

        VacancyViewModel GetVacancy(int vacancyReferenceNumber);

        List<VacancyLocationAddressViewModel> GetLocationsAddressViewModelsByReferenceNumber(int vacancyReferenceNumber);

        List<VacancyLocationAddressViewModel> GetLocationsAddressViewModels(int vacancyId);

        VacancyViewModel SubmitVacancy(int vacancyReferenceNumber);

        List<SelectListItem> GetSectorsAndFrameworks();

        List<StandardViewModel> GetStandards();

        List<SelectListItem> GetSectors();

        StandardViewModel GetStandard(int? standardId);

        VacanciesSummaryViewModel GetVacanciesSummaryForProvider(int providerId, int providerSiteId, VacanciesSummarySearchViewModel vacanciesSummarySearch);

        VacancyOwnerRelationshipViewModel CloneVacancy(int vacancyReferenceNumber);

        LocationSearchViewModel LocationAddressesViewModel(string ukprn, int providerSiteId, int employerId, Guid vacancyGuid, bool isAnonymousEmployer = false);

        VacancyViewModel GetVacancy(Guid vacancyReferenceNumber);

        void RemoveVacancyLocationInformation(Guid vacancyGuid);

        void RemoveLocationAddresses(Guid vacancyGuid);

        LocationSearchViewModel AddLocations(LocationSearchViewModel viewModel);

        void EmptyVacancyLocation(int vacancyReferenceNumber);
        void CreateVacancy(VacancyMinimumData vacancyMinimumData);
        void TransferVacancies(ManageVacancyTransferViewModel vacancyTransferViewModel);
        FurtherVacancyDetailsViewModel CloseVacancy(FurtherVacancyDetailsViewModel viewModel);
    }
}
