namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using System.Collections.Generic;
    using Common.Mediators;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;

    public interface IVacancyMediator
    {
        MediatorResponse<DashboardVacancySummaryViewModel> ApproveVacancy(long vacancyReferenceNumber);

        MediatorResponse<DashboardVacancySummaryViewModel> RejectVacancy(long vacancyReferenceNumber);

        MediatorResponse<VacancyViewModel> ReserveVacancyForQA(long vacancyReferenceNumber);

        MediatorResponse<VacancySummaryViewModel> GetVacancySummaryViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancy(VacancySummaryViewModel viewModel);

        MediatorResponse<NewVacancyViewModel> GetBasicDetails(long vacancyReferenceNumber);

        MediatorResponse<TrainingDetailsViewModel> GetTrainingDetails(long vacancyReferenceNumber);

        MediatorResponse<TrainingDetailsViewModel> UpdateVacancy(TrainingDetailsViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancyRequirementsProspectsViewModel> GetVacancyRequirementsProspectsViewModel(
            long vacancyReferenceNumber);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> GetEmployerInformation(long vacancyReferenceNumber,
            bool? useEmployerLocation);

        MediatorResponse<NewVacancyViewModel> UpdateVacancy(NewVacancyViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(
            VacancyRequirementsProspectsViewModel viewModel);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> UpdateEmployerInformation(
            ProviderSiteEmployerLinkViewModel viewModel);

        MediatorResponse<LocationSearchViewModel> GetLocationAddressesViewModel(long vacancyReferenceNumber);

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