namespace SFA.Apprenticeships.Web.Recruit.Mediators.Admin
{
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Common.Mediators;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using System;
    using ViewModels.Admin;

    public class AdminMediator : MediatorBase, IAdminMediator
    {
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IProviderService _providerService;

        public AdminMediator(IVacancyPostingService vacancyPostingService, IProviderService providerService)
        {
            _vacancyPostingService = vacancyPostingService;
            _providerService = providerService;
        }

        public MediatorResponse<TransferVacanciesViewModel> GetVacancyDetails(TransferVacanciesViewModel viewModel)
        {
            foreach (var vacancy in viewModel.VacancyReferenceNumbers.Split(','))
            {
                string vacancyReference;
                if (VacancyHelper.TryGetVacancyReference(vacancy, out vacancyReference))
                {
                    Vacancy vacancyDetails = _vacancyPostingService.GetVacancyByReferenceNumber(Convert.ToInt32(vacancyReference));
                    TransferVacancyViewModel vacancyView = new TransferVacancyViewModel
                    {
                        ContractOwnerId = vacancyDetails.ProviderId,
                        VacancyManagerId = vacancyDetails.VacancyManagerId,
                        VacancyReferenceNumber = vacancyDetails.VacancyReferenceNumber,
                        DeliveryOrganisationId = vacancyDetails.DeliveryOrganisationId,
                        VacancyOwnerRelationShipId = vacancyDetails.VacancyOwnerRelationshipId,
                        ProviderName = _providerService.GetProvider(vacancyDetails.ProviderId).TradingName,
                    };
                    if (vacancyDetails.VacancyManagerId.HasValue)
                    {
                        vacancyView.ProviderSiteName =
                            _providerService.GetProviderSite(vacancyDetails.VacancyManagerId.Value).TradingName;
                    }
                    viewModel.VacanciesToBeTransferredVm.Add(vacancyView);
                }
            }
            return GetMediatorResponse(AdminMediatorCodes.GetVacancyDetails.Ok, viewModel);
        }
    }
}