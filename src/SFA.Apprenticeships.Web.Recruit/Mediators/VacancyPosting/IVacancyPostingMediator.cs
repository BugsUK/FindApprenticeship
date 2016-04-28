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
        MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(int providerSiteId, Guid? vacancyGuid, bool? comeFromPreview);

        MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(EmployerSearchViewModel employerFilterViewModel);

        MediatorResponse<EmployerSearchViewModel> GetEmployers(EmployerSearchViewModel employerFilterViewModel);

        MediatorResponse<VacancyPartyViewModel> GetEmployer(int providerSiteId, string edsUrn, Guid vacancyGuid, bool? comeFromPreview, bool? useEmployerLocation);

        MediatorResponse<VacancyPartyViewModel> ConfirmEmployer(VacancyPartyViewModel viewModel, string ukprn);

        MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(int vacancyPartyId, Guid vacancyGuid, int? numberOfPositions);

        MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(int vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<NewVacancyViewModel> CreateVacancyAndExit(NewVacancyViewModel newVacancyViewModel, string ukprn);

        MediatorResponse<NewVacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel, string ukprn);

        MediatorResponse<TrainingDetailsViewModel> GetTrainingDetailsViewModel(int vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<TrainingDetailsViewModel> UpdateVacancy(TrainingDetailsViewModel viewModel);

        MediatorResponse<TrainingDetailsViewModel> UpdateVacancyAndExit(TrainingDetailsViewModel viewModel);

        MediatorResponse<FurtherVacancyDetailsViewModel> GetVacancySummaryViewModel(int vacancyReferenceNumber, bool validate, bool? comeFromPreview);

        MediatorResponse<FurtherVacancyDetailsViewModel> UpdateVacancy(FurtherVacancyDetailsViewModel viewModel, bool acceptWarnings);

        MediatorResponse<FurtherVacancyDetailsViewModel> UpdateVacancyAndExit(FurtherVacancyDetailsViewModel viewModel);

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

        MediatorResponse<VacancyPartyViewModel> CloneVacancy(int vacancyReferenceNumber);
		
        MediatorResponse<LocationSearchViewModel> AddLocations(LocationSearchViewModel newVacancyViewModel, string ukprn);

        MediatorResponse<LocationSearchViewModel> GetLocationAddressesViewModel(int providerSiteId, int employerId, string ukprn, Guid vacancyGuid, bool? comeFromPreview);

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