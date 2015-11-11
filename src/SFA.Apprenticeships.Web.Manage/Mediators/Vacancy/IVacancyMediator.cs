using SFA.Apprenticeships.Web.Raa.Common.ViewModels;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using Common.Mediators;
    using ViewModels;

    public interface IVacancyMediator
    {
        MediatorResponse<DashboardVacancySummaryViewModel> ApproveVacancy(long vacancyReferenceNumber);

        MediatorResponse<DashboardVacancySummaryViewModel> RejectVacancy(long vacancyReferenceNumber);

        MediatorResponse<VacancyViewModel> ReserveVacancyForQA(long vacancyReferenceNumber);

        MediatorResponse<VacancySummaryViewModel> GetVacancySummaryViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancy(VacancySummaryViewModel viewModel, bool acceptWarnings);

        MediatorResponse<NewVacancyViewModel> GetBasicDetails(long vacancyReferenceNumber);

        MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel);

        MediatorResponse<NewVacancyViewModel> UpdateVacancy(NewVacancyViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);
    }
}