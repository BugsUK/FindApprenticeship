namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyManagement
{
    using System;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Vacancy;
    using Common.Constants;
    using Common.Mediators;
    using Domain.Entities.Raa.Vacancies;
    using Mappers;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.VacancyManagement;
    using VacancyPosting;

    public class VacancyManagementMediator : MediatorBase, IVacancyManagementMediator
    {
        private static readonly IMapper RecruitMappers = new RecruitMappers();

        private readonly IVacancyManagementProvider _vacancyManagementProvider;

        public VacancyManagementMediator(IVacancyManagementProvider vacancyManagementProvider)
        {
            _vacancyManagementProvider = vacancyManagementProvider;
        }

        public MediatorResponse<DeleteVacancyViewModel> Delete(DeleteVacancyViewModel vacancyViewModel)
        {
            MediatorResponseMessage message;

            var result = _vacancyManagementProvider.Delete(vacancyViewModel.VacancyId);
            var vacancyTitle = string.IsNullOrEmpty(vacancyViewModel.VacancyTitle) ? "(No Title)" : vacancyViewModel.VacancyTitle;
            if (result.Code == VacancyManagementServiceCodes.Delete.Ok)
            {
                message = new MediatorResponseMessage {Text = $"You have deleted {vacancyTitle} vacancy"};
                return GetMediatorResponse(VacancyManagementMediatorCodes.DeleteVacancy.Ok, vacancyViewModel, null, null, message);
            }

            message = new MediatorResponseMessage { Text = $"There was a problem deleting {vacancyTitle} vacancy", Level = UserMessageLevel.Error };
            return GetMediatorResponse(VacancyManagementMediatorCodes.DeleteVacancy.Failure, vacancyViewModel, null, null, message);
        }

        public MediatorResponse<DeleteVacancyViewModel> ConfirmDelete(DeleteVacancyViewModel vacancyViewModel)
        {
            var serviceResult = _vacancyManagementProvider.FindSummary(vacancyViewModel.VacancyId);
            if (serviceResult.Code == VacancyManagementServiceCodes.FindSummary.Ok)
            {
                vacancyViewModel.VacancyTitle = serviceResult.Result.Title;
                return GetMediatorResponse(VacancyManagementMediatorCodes.ConfirmDelete.Ok, vacancyViewModel);
            }
            if (serviceResult.Code == VacancyManagementServiceCodes.FindSummary.NotFound)
            {
                return GetMediatorResponse(VacancyManagementMediatorCodes.ConfirmDelete.NotFound, vacancyViewModel);
            }

            return GetMediatorResponse(serviceResult.Code, vacancyViewModel);
        }

        public MediatorResponse<EditWageViewModel> EditWage(int vacancyReferenceNumber)
        {
            var serviceResult = _vacancyManagementProvider.FindSummaryByReferenceNumber(vacancyReferenceNumber);
            if (serviceResult.Code == VacancyManagementServiceCodes.FindSummary.Ok)
            {
                var viewModel = RecruitMappers.Map<VacancySummary, EditWageViewModel>(serviceResult.Result);
                return GetMediatorResponse(VacancyManagementMediatorCodes.EditWage.Ok, viewModel);
            }
            if (serviceResult.Code == VacancyManagementServiceCodes.FindSummary.NotFound)
            {
                var viewModel = new EditWageViewModel
                {
                    VacancyReferenceNumber = vacancyReferenceNumber
                };
                return GetMediatorResponse(VacancyManagementMediatorCodes.EditWage.NotFound, viewModel);
            }

            throw new ArgumentException($"serviceResult: {serviceResult} from _vacancyManagementProvider.FindSummary({vacancyReferenceNumber}); was not recognised");
        }
    }
}