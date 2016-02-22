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
        NewVacancyViewModel GetNewVacancyViewModel(long vacancyReferenceNumber);

        NewVacancyViewModel GetNewVacancyViewModel(int vacancyPartyId, Guid vacancyGuid, int? numberOfPositions);

        NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        TrainingDetailsViewModel GetTrainingDetailsViewModel(long vacancyReferenceNumber);

        TrainingDetailsViewModel UpdateVacancy(TrainingDetailsViewModel viewModel);

        FurtherVacancyDetailsViewModel GetVacancySummaryViewModel(long vacancyReferenceNumber);

        VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(long vacancyReferenceNumber);

        VacancyQuestionsViewModel GetVacancyQuestionsViewModel(long vacancyReferenceNumber);

        VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel);

        FurtherVacancyDetailsViewModel UpdateVacancy(FurtherVacancyDetailsViewModel viewModel);

        VacancyRequirementsProspectsViewModel UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);

        VacancyViewModel GetVacancy(long vacancyReferenceNumber);

        VacancyViewModel SubmitVacancy(long vacancyReferenceNumber);

        List<SelectListItem> GetSectorsAndFrameworks();

        List<StandardViewModel> GetStandards();

        List<SelectListItem> GetSectors();

        StandardViewModel GetStandard(int? standardId);

        VacanciesSummaryViewModel GetVacanciesSummaryForProvider(int providerId, int providerSiteId, VacanciesSummarySearchViewModel vacanciesSummarySearch);

        VacancyPartyViewModel CloneVacancy(long vacancyReferenceNumber);

        LocationSearchViewModel CreateVacancy(LocationSearchViewModel newVacancyViewModel);

        LocationSearchViewModel LocationAddressesViewModel(string ukprn, int providerSiteId, int employerId, Guid vacancyGuid);

        VacancyViewModel GetVacancy(Guid vacancyReferenceNumber);

        void RemoveVacancyLocationInformation(Guid vacancyGuid);

        void RemoveLocationAddresses(Guid vacancyGuid);

        LocationSearchViewModel AddLocations(LocationSearchViewModel viewModel);

        VacancyDatesViewModel GetVacancyDatesViewModel(long vacancyReferenceNumber);

        VacancyDatesViewModel UpdateVacancy(VacancyDatesViewModel viewModel);
    }
}
