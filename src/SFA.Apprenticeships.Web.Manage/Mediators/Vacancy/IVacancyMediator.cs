using SFA.Apprenticeships.Web.Raa.Common.ViewModels;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.VacancyPosting;

    public interface IVacancyMediator
    {
        MediatorResponse<DashboardVacancySummaryViewModel> ApproveVacancy(long vacancyReferenceNumber);

        MediatorResponse<DashboardVacancySummaryViewModel> RejectVacancy(long vacancyReferenceNumber);

        MediatorResponse<VacancyViewModel> ReserveVacancyForQA(long vacancyReferenceNumber);

        MediatorResponse<VacancySummaryViewModel> GetVacancySummaryViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancy(VacancySummaryViewModel viewModel);

        MediatorResponse<NewVacancyViewModel> GetBasicDetails(long vacancyReferenceNumber);

        MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancyRequirementsProspectsViewModel> GetVacancyRequirementsProspectsViewModel(long vacancyReferenceNumber);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> GetEmployerInformation(long vacancyReferenceNumber, bool? useEmployerLocation);

        MediatorResponse<NewVacancyViewModel> UpdateVacancy(NewVacancyViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> UpdateEmployerInformation(ProviderSiteEmployerLinkViewModel viewModel);

        MediatorResponse<LocationSearchViewModel> GetLocationAddressesViewModel(long vacancyReferenceNumber);

        MediatorResponse<LocationSearchViewModel> AddLocations(LocationSearchViewModel viewModel);
    }
}