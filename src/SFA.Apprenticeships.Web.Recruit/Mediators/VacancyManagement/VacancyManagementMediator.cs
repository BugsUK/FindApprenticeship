namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyManagement
{
    using System;
    using System.Threading.Tasks;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Vacancy;
    using Common.Constants;
    using Common.Mappers.Resolvers;
    using Common.Mediators;
    using Common.ViewModels;
    using Mappers;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.VacancyManagement;
    using VacancyPosting;
    using Validators;
    using VacancySummary = Domain.Entities.Raa.Vacancies.VacancySummary;

    public class VacancyManagementMediator : MediatorBase, IVacancyManagementMediator
    {
        private static readonly IMapper RecruitMappers = new RecruitMappers();

        private readonly EditWageViewModelValidator _editWageViewModelValidator = new EditWageViewModelValidator();

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
                var vacancySummary = serviceResult.Result;
                var viewModel = RecruitMappers.Map<VacancySummary, EditWageViewModel>(vacancySummary);
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

        public async Task<MediatorResponse<EditWageViewModel>> EditWage(EditWageViewModel editWageViewModel)
        {
            var serviceResult = _vacancyManagementProvider.FindSummaryByReferenceNumber(editWageViewModel.VacancyReferenceNumber);
            if (serviceResult.Code == VacancyManagementServiceCodes.FindSummary.Ok)
            {
                var vacancySummary = serviceResult.Result;
                var viewModel = RecruitMappers.Map<VacancySummary, EditWageViewModel>(vacancySummary);

                if (editWageViewModel.Classification == WageClassification.LegacyText)
                {
                    //Hack to cope with the fact that WageClassification.NotApplicable is -1 rather than 0
                    editWageViewModel.Classification = WageClassification.NotApplicable;
                }
                editWageViewModel.ExistingWage = viewModel.ExistingWage;
                editWageViewModel.Type = editWageViewModel.Classification == WageClassification.NotApplicable || (editWageViewModel.Classification == WageClassification.Custom && editWageViewModel.CustomType == CustomWageType.NotApplicable)
                    ? editWageViewModel.Type = viewModel.ExistingWage.Type
                    : WageViewModelToWageConverter.GetWageType(editWageViewModel.Classification,
                        editWageViewModel.CustomType, PresetText.NotApplicable);
                editWageViewModel.Unit = WageViewModelToWageConverter.GetWageUnit(editWageViewModel.Classification, editWageViewModel.CustomType, editWageViewModel.Unit ?? editWageViewModel.ExistingWage.Unit, editWageViewModel.RangeUnit);
                editWageViewModel.PossibleStartDate = viewModel.PossibleStartDate;

                var validationResult = _editWageViewModelValidator.Validate(editWageViewModel);
                if (validationResult.IsValid)
                {
                    var editResult = await _vacancyManagementProvider.EditWage(editWageViewModel);
                    if (editResult.Code == VacancyManagementServiceCodes.EditWage.Ok)
                    {
                        return GetMediatorResponse(VacancyManagementMediatorCodes.EditWage.Ok, editResult.Result);
                    }
                    return GetMediatorResponse(VacancyManagementMediatorCodes.EditWage.Failure, editWageViewModel);
                }
                return GetMediatorResponse(VacancyManagementMediatorCodes.EditWage.FailedValidation, editWageViewModel, validationResult);
            }
            if (serviceResult.Code == VacancyManagementServiceCodes.FindSummary.NotFound)
            {
                return GetMediatorResponse(VacancyManagementMediatorCodes.EditWage.NotFound, editWageViewModel);
            }

            throw new ArgumentException($"serviceResult: {serviceResult} from _vacancyManagementProvider.FindSummary({editWageViewModel.VacancyReferenceNumber}); was not recognised");
        }
    }
}