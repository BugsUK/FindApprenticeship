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

        MediatorResponse<ProviderSiteEmployerLinkViewModel> GetEmployer(int providerSiteId, int employerId, Guid vacancyGuid, bool? comeFromPreview, bool? useEmployerLocation);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> ConfirmEmployer(ProviderSiteEmployerLinkViewModel viewModel);

        MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(int providerId, int providerSiteId, int employerId, Guid vacancyGuid, int? numberOfPositions);

        MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(long vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<NewVacancyViewModel> CreateVacancyAndExit(NewVacancyViewModel newVacancyViewModel);

        MediatorResponse<NewVacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        MediatorResponse<TrainingDetailsViewModel> GetTrainingDetailsViewModel(long vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<TrainingDetailsViewModel> UpdateVacancy(TrainingDetailsViewModel viewModel);

        MediatorResponse<TrainingDetailsViewModel> UpdateVacancyAndExit(TrainingDetailsViewModel viewModel);

        MediatorResponse<VacancySummaryViewModel> GetVacancySummaryViewModel(long vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancy(VacancySummaryViewModel viewModel, bool acceptWarnings);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancyAndExit(VacancySummaryViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> GetVacancyRequirementsProspectsViewModel(long vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancyAndExit(VacancyRequirementsProspectsViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(long vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancyAndExit(VacancyQuestionsViewModel viewModel);

        MediatorResponse<VacancyViewModel> SubmitVacancy(long vacancyReferenceNumber, bool resubmitOptin);

        MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(long vacancyReferenceNumber, bool resubmitted);

        MediatorResponse<EmployerSearchViewModel> SelectNewEmployer(EmployerSearchViewModel viewModel);

        MediatorResponse<VacancyViewModel> GetPreviewVacancyViewModel(long vacancyReferenceNumber);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> CloneVacancy(long vacancyReferenceNumber);
		
        MediatorResponse<LocationSearchViewModel> AddLocations(LocationSearchViewModel newVacancyViewModel);

        MediatorResponse<LocationSearchViewModel> GetLocationAddressesViewModel(string providerSiteErn, string ern, string ukprn, Guid vacancyGuid, bool? comeFromPreview);

        MediatorResponse<LocationSearchViewModel> SearchLocations(LocationSearchViewModel viewModel, List<VacancyLocationAddressViewModel> alreadyAddedLocations);

        MediatorResponse<LocationSearchViewModel> UseLocation(LocationSearchViewModel viewModel, int locationIndex,
            string postCodeSearch);

        MediatorResponse<LocationSearchViewModel> RemoveLocation(LocationSearchViewModel viewModel, int locationIndex);

        MediatorResponse<TrainingDetailsViewModel> SelectFrameworkAsTrainingType(TrainingDetailsViewModel viewModel);
		
        MediatorResponse ClearLocationInformation(Guid vacancyGuid);
		
        MediatorResponse<TrainingDetailsViewModel> SelectStandardAsTrainingType(TrainingDetailsViewModel viewModel);

        MediatorResponse<VacancyDatesViewModel> GetVacancyDatesViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancyDatesViewModel> UpdateVacancy(VacancyDatesViewModel viewModel, bool acceptWarnings);
    }
}