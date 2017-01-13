namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Interfaces.Service;
    using Application.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using Mappers;
    using ViewModels.VacancyManagement;
    using WageUpdate = DAS.RAA.Api.Client.V1.Models.WageUpdate;

    public class VacancyManagementProvider : IVacancyManagementProvider
    {
        private static readonly IMapper ApiClientMappers = new ApiClientMappers();

        private readonly IVacancyManagementService _vacancyManagementService;
        private readonly IApiClientProvider _apiClientProvider;

        public VacancyManagementProvider(IVacancyManagementService vacancyManagementService, IApiClientProvider apiClientProvider)
        {
            _vacancyManagementService = vacancyManagementService;
            _apiClientProvider = apiClientProvider;
        }

        public IServiceResult Delete(int vacancyId)
        {
            return new ServiceResult(_vacancyManagementService.Delete(vacancyId).Code);
        }

        public IServiceResult<VacancySummary> FindSummary(int vacancyId)
        {
            return _vacancyManagementService.FindSummary(vacancyId);
        }

        public IServiceResult<VacancySummary> FindSummaryByReferenceNumber(int vacancyReferenceNumber)
        {
            return _vacancyManagementService.FindSummaryByReferenceNumber(vacancyReferenceNumber);
        }

        public async Task<IServiceResult<EditWageViewModel>> EditWage(EditWageViewModel editWageViewModel)
        {
            var wageUpdate = ApiClientMappers.Map<EditWageViewModel, WageUpdate>(editWageViewModel);

            var apiClient = _apiClientProvider.GetApiClient();

            var apiVacancyResult = await apiClient.EditVacancyWageWithHttpMessagesAsync(wageUpdate, vacancyReferenceNumber: editWageViewModel.VacancyReferenceNumber);
            if (apiVacancyResult.Response.IsSuccessStatusCode)
            {
                return new ServiceResult<EditWageViewModel>(VacancyManagementServiceCodes.EditWage.Ok, editWageViewModel);
            }

            return new ServiceResult<EditWageViewModel>(VacancyManagementServiceCodes.EditWage.Error, editWageViewModel);
        }
    }
}