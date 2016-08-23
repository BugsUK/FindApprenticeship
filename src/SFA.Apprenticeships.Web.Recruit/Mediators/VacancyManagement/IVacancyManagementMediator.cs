namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyManagement
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Vacancy;

    public interface IVacancyManagementMediator
    {
        MediatorResponse<DeleteVacancyViewModel> Delete(DeleteVacancyViewModel vacancyViewModel);
        MediatorResponse<DeleteVacancyViewModel> ConfirmDelete(DeleteVacancyViewModel vacancyViewModel);
    }
}