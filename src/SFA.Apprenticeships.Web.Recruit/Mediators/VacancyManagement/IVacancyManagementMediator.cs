namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyManagement
{
    using Common.Mediators;
    using Raa.Common.ViewModels.VacancyManagement;

    public interface IVacancyManagementMediator
    {
        MediatorResponse<DeleteVacancyViewModel> Delete(DeleteVacancyViewModel vacancyViewModel);
        MediatorResponse<DeleteVacancyViewModel> ConfirmDelete(DeleteVacancyViewModel vacancyViewModel);
        MediatorResponse<EditWageViewModel> EditWage(int vacancyReferenceNumber);
        MediatorResponse<EditWageViewModel> EditWage(EditWageViewModel editWageViewModel);
    }
}