namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using System.Collections.Generic;
    using Common.Mediators;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;

    public interface IVacancyMediator
    {
        MediatorResponse<DashboardVacancySummaryViewModel> ApproveVacancy(int vacancyReferenceNumber);

        MediatorResponse<DashboardVacancySummaryViewModel> RejectVacancy(int vacancyReferenceNumber);

        MediatorResponse<VacancyViewModel> ReserveVacancyForQA(int vacancyReferenceNumber);

        MediatorResponse<VacancySummaryViewModel> GetVacancySummaryViewModel(int vacancyReferenceNumber);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancy(VacancySummaryViewModel viewModel);

        MediatorResponse<NewVacancyViewModel> GetBasicDetails(int vacancyReferenceNumber);

        MediatorResponse<TrainingDetailsViewModel> GetTrainingDetails(int vacancyReferenceNumber);

        MediatorResponse<TrainingDetailsViewModel> UpdateVacancy(TrainingDetailsViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(int vacancyReferenceNumber);

        MediatorResponse<VacancyRequirementsProspectsViewModel> GetVacancyRequirementsProspectsViewModel(
            int vacancyReferenceNumber);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> GetEmployerInformation(int vacancyReferenceNumber,
            bool? useEmployerLocation);

        MediatorResponse<NewVacancyViewModel> UpdateVacancy(NewVacancyViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(
            VacancyRequirementsProspectsViewModel viewModel);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> UpdateEmployerInformation(
            ProviderSiteEmployerLinkViewModel viewModel);

        MediatorResponse<LocationSearchViewModel> GetLocationAddressesViewModel(int vacancyReferenceNumber);

        MediatorResponse<LocationSearchViewModel> AddLocations(LocationSearchViewModel viewModel);

        MediatorResponse<LocationSearchViewModel> SearchLocations(LocationSearchViewModel viewModel,
            List<VacancyLocationAddressViewModel> alreadyAddedLocations);

        MediatorResponse<LocationSearchViewModel> UseLocation(LocationSearchViewModel viewModel, int locationIndex,
            string postCodeSearch);

        MediatorResponse<LocationSearchViewModel> RemoveLocation(LocationSearchViewModel viewModel, int locationIndex);

        MediatorResponse<TrainingDetailsViewModel> SelectFrameworkAsTrainingType(TrainingDetailsViewModel viewModel);

        MediatorResponse<TrainingDetailsViewModel> SelectStandardAsTrainingType(TrainingDetailsViewModel viewModel);
    }
}