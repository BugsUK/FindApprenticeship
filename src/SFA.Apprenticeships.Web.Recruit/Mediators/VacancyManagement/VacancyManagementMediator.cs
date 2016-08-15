namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyManagement
{
    using System;
    using Apprenticeships.Application.Vacancy;
    using Common.Mediators;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Vacancy;
    using VacancyPosting;

    public class VacancyManagementMediator : MediatorBase, IVacancyManagementMediator
    {
        private readonly IVacancyManagementProvider _vacancyManagementProvider;

        public VacancyManagementMediator(IVacancyManagementProvider vacancyManagementProvider)
        {
            _vacancyManagementProvider = vacancyManagementProvider;
        }

        public MediatorResponse<DeleteVacancyViewModel> Delete(DeleteVacancyViewModel vacancyViewModel)
        {
            _vacancyManagementProvider.Delete(vacancyViewModel.VacancyId);
            return GetMediatorResponse(VacancyManagementMediatorCodes.DeleteVacancy.Ok, vacancyViewModel);
        }

        public MediatorResponse<DeleteVacancyViewModel> ConfirmDelete(DeleteVacancyViewModel vacancyViewModel)
        {
            var serviceResult = _vacancyManagementProvider.FindSummary(vacancyViewModel.VacancyId);
            if (serviceResult.Code == VacancyManagementServiceCodes.FindSummary.Ok)
            {
                vacancyViewModel.VacancyTitle = serviceResult.Result.Title;
                return GetMediatorResponse(VacancyManagementMediatorCodes.ConfirmDelete.Ok, vacancyViewModel);
            }

            return GetMediatorResponse(serviceResult.Code, vacancyViewModel);
        }
    }
}