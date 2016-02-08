namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using Common.Mediators;
    using System;
    using System.Collections.Generic;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;

    public interface IVacancyPostingMediator
    {
        MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(string providerSiteErn, Guid? vacancyGuid, bool? comeFromPreview);

        MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(EmployerSearchViewModel employerFilterViewModel);

        MediatorResponse<EmployerSearchViewModel> GetEmployers(EmployerSearchViewModel employerFilterViewModel);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> GetEmployer(string providerSiteErn, string ern, Guid vacancyGuid, bool? comeFromPreview, bool? useEmployerLocation);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> ConfirmEmployer(ProviderSiteEmployerLinkViewModel viewModel);

        MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(string ukprn, string providerSiteErn, string ern, Guid vacancyGuid, int? numberOfPositions);

        MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(int vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<NewVacancyViewModel> CreateVacancyAndExit(NewVacancyViewModel newVacancyViewModel);

        MediatorResponse<NewVacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        MediatorResponse<TrainingDetailsViewModel> GetTrainingDetailsViewModel(int vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<TrainingDetailsViewModel> UpdateVacancy(TrainingDetailsViewModel viewModel);

        MediatorResponse<TrainingDetailsViewModel> UpdateVacancyAndExit(TrainingDetailsViewModel viewModel);

        MediatorResponse<VacancySummaryViewModel> GetVacancySummaryViewModel(int vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancy(VacancySummaryViewModel viewModel, bool acceptWarnings);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancyAndExit(VacancySummaryViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> GetVacancyRequirementsProspectsViewModel(int vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancyAndExit(VacancyRequirementsProspectsViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(int vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancyAndExit(VacancyQuestionsViewModel viewModel);

        MediatorResponse<VacancyViewModel> SubmitVacancy(int vacancyReferenceNumber, bool resubmitOptin);

        MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(int vacancyReferenceNumber, bool resubmitted);

        MediatorResponse<EmployerSearchViewModel> SelectNewEmployer(EmployerSearchViewModel viewModel);

        MediatorResponse<VacancyViewModel> GetPreviewVacancyViewModel(int vacancyReferenceNumber);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> CloneVacancy(int vacancyReferenceNumber);
		
        MediatorResponse<LocationSearchViewModel> AddLocations(LocationSearchViewModel newVacancyViewModel);

        MediatorResponse<LocationSearchViewModel> GetLocationAddressesViewModel(string providerSiteErn, string ern, string ukprn, Guid vacancyGuid, bool? comeFromPreview);

        MediatorResponse<LocationSearchViewModel> SearchLocations(LocationSearchViewModel viewModel, List<VacancyLocationAddressViewModel> alreadyAddedLocations);

        MediatorResponse<LocationSearchViewModel> UseLocation(LocationSearchViewModel viewModel, int locationIndex,
            string postCodeSearch);

        MediatorResponse<LocationSearchViewModel> RemoveLocation(LocationSearchViewModel viewModel, int locationIndex);

        MediatorResponse<TrainingDetailsViewModel> SelectFrameworkAsTrainingType(TrainingDetailsViewModel viewModel);
		
        MediatorResponse ClearLocationInformation(Guid vacancyGuid);
		
        MediatorResponse<TrainingDetailsViewModel> SelectStandardAsTrainingType(TrainingDetailsViewModel viewModel);

        MediatorResponse<VacancyDatesViewModel> GetVacancyDatesViewModel(int vacancyReferenceNumber);

        MediatorResponse<VacancyDatesViewModel> UpdateVacancy(VacancyDatesViewModel viewModel, bool acceptWarnings);
    }
}